using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Linq;

using Npgsql;

using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.Extensions;

namespace Simple.Data.Npgsql
{
  [Export(typeof(IBulkInserter))]
  public class PgBulkInserter : IBulkInserter
  {
    public IEnumerable<IDictionary<string, object>> Insert(AdoAdapter adapter, string tableName, IEnumerable<IDictionary<string, object>> data, IDbTransaction transaction, Func<IDictionary<string,object>, Exception, bool> onError, bool resultRequired)
    {
      var table = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindTable(tableName);
      if (table == null) throw new SimpleDataException($"Table '{tableName}' not found");

      var insertData = data.Select(row => row.Where(p => table.HasColumn(p.Key) && !table.FindColumn(p.Key).IsIdentity).ToDictionary());

      var insertColumns = insertData.First().Keys.Select(table.FindColumn).ToArray();

      var columnsSql = insertColumns.Select(s => s.QuotedName).Aggregate((agg, next) => String.Concat(agg, ",", next));
      var valuesSql = insertColumns.Select((val, idx) => ":p" + idx.ToString()).Aggregate((agg, next) => String.Concat(agg, ",", next));

      var insertSql = $"INSERT INTO {table.QualifiedName} ({columnsSql}) VALUES({valuesSql}){(resultRequired ? " RETURNING *" : "")};";

      if (transaction != null)
      {
        using(var cmd = transaction.Connection.CreateCommand())
        {
          cmd.Transaction = transaction;
          cmd.CommandText = insertSql;
          if (resultRequired)
          {
            return insertData.Select(row => ExecuteInsert(cmd, insertColumns, row, onError)).ToList();
          }
          else
          {
            insertData.Select(row => ExecuteInsert(cmd, insertColumns, row, onError)).ToList();
            return null;
          }
        }
      }

      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        conn.Open();
        using(var cmd = conn.CreateCommand())
        {
          cmd.CommandText = insertSql;
          if (resultRequired)
          {
            return insertData.Select(row => ExecuteInsert(cmd, insertColumns, row, onError)).ToList();
          }
          else
          {
            insertData.Select(row => ExecuteInsert(cmd, insertColumns, row, onError)).ToList();
            return null;
          }
        }
      }
    }

    private IDictionary<string, object> ExecuteInsert(IDbCommand cmd, Column[] insertColumns, IDictionary<string, object> insertData, Func<IDictionary<string, object>, Exception, bool> onError)
    {
      AddCommandParameters(cmd, insertColumns, insertData.Values.ToArray());
      cmd.WriteTrace();
      try
      {
        using (var rdr = cmd.ExecuteReader())
        {
          if (rdr.Read())
          {
            return rdr.ToDictionary();
          }
        }
      }
      catch (DbException ex)
      {
        if (onError(insertData, ex)) return null;
        throw new AdoAdapterException(ex.Message, cmd);
      }

      return null;
    }

    private void AddCommandParameters(IDbCommand cmd, Column[] insertColumns, object[] insertData)
    {
      cmd.Parameters.Clear();
      for (var idx = 0; idx < insertColumns.Length; idx++)
      {
        object value = InsertParameter.Transform(insertData[idx]);
        var parameter = new NpgsqlParameter
                          {
                            ParameterName = string.Concat("p", idx.ToString()),
                            Value = value
                          };
        cmd.Parameters.Add(parameter);
      }
    }

  }
}
