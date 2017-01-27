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
  [Export(typeof(ICustomInserter))]
  public class PgCustomInserter : ICustomInserter
  {
    public IDictionary<string, object> Insert(AdoAdapter adapter, string tableName, IDictionary<string, object> data, IDbTransaction transaction, bool resultRequired = false)
    {
      var table = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindTable(tableName);
      if (table == null) throw new SimpleDataException($"Table '{tableName}' not found");

      var insertData = data.Where(p => table.HasColumn(p.Key) && !table.FindColumn(p.Key).IsIdentity).ToDictionary();

      var insertColumns = insertData.Keys.Select(table.FindColumn).ToArray();

      var columnsSql = insertColumns.Select(s => s.QuotedName).Aggregate((agg, next) => String.Concat(agg, ",", next));
      var valuesSql = insertColumns.Select((val, idx) => ":p" + idx.ToString()).Aggregate((agg, next) => String.Concat(agg, ",", next));

      var insertSql = $"INSERT INTO {table.QualifiedName} ({columnsSql}) VALUES({valuesSql}) RETURNING *;";
      if (transaction != null)
      {
        using(var cmd = transaction.Connection.CreateCommand())
        {
          cmd.Transaction = transaction;
          cmd.CommandText = insertSql;
          return ExecuteInsert(cmd, insertColumns, insertData.Values.ToArray());
        }
      }

      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        conn.Open();
        using(var cmd = conn.CreateCommand())
        {
          cmd.CommandText = insertSql;
          return ExecuteInsert(cmd, insertColumns, insertData.Values.ToArray());
        }
      }
    }
    
    private IDictionary<string, object> ExecuteInsert(IDbCommand cmd, Column[] insertColumns, object[] insertData)
    {
      AddCommandParameters(cmd, insertColumns, insertData);
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
                            ParameterName = String.Concat("p", idx.ToString()),
                            Value = value
                          };
        cmd.Parameters.Add(parameter);
      }
    }
  }
}