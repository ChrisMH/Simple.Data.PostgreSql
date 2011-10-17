using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Npgsql;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.Extensions;

namespace Simple.Data.PostgreSql
{
  [Export(typeof(IBulkInserter))]
  public class PgBulkInserter : IBulkInserter
  {
    public IEnumerable<IDictionary<string, object>> Insert(AdoAdapter adapter, string tableName, IEnumerable<IDictionary<string, object>> data, IDbTransaction transaction)
    {
      var table = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindTable(tableName);
      if (table == null) throw new SimpleDataException(String.Format("Table '{0}' not found", tableName));

      var insertData = data.Select(row => row.Where(p => table.HasColumn(p.Key) && !table.FindColumn(p.Key).IsIdentity).ToDictionary());

      var insertColumns = insertData.First().Keys.Select(table.FindColumn).ToArray();

      var columnsSql = insertColumns.Select(s => s.QuotedName).Aggregate((agg, next) => String.Concat(agg, ",", next));
      var valuesSql = insertColumns.Select((val, idx) => ":p" + idx.ToString()).Aggregate((agg, next) => String.Concat(agg, ",", next));

      var insertSql = string.Format("INSERT INTO {0} ({1}) VALUES({2}) RETURNING *;", table.QualifiedName, columnsSql, valuesSql);
      if (transaction != null)
      {
        using(var cmd = transaction.Connection.CreateCommand())
        {
          cmd.Transaction = transaction;
          cmd.CommandText = insertSql;
          return insertData.Select(row => ExecuteInsert(cmd, insertColumns, row.Values.ToArray())).ToList();
        }
      }

      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        conn.Open();
        using(var cmd = conn.CreateCommand())
        {
          cmd.CommandText = insertSql;
          return insertData.Select(row => ExecuteInsert(cmd, insertColumns, row.Values.ToArray())).ToList();
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
        var parameter = new NpgsqlParameter
                          {
                            ParameterName = String.Concat("p", idx.ToString()),
                            DbType = insertColumns[idx].DbType,
                            Value = insertData[idx]
                          };
        cmd.Parameters.Add(parameter);
      }
    }

  }
}
