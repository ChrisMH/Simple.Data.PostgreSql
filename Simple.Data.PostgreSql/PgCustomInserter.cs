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
  [Export(typeof(ICustomInserter))]
  public class PgCustomInserter : ICustomInserter
  {
    public IDictionary<string, object> Insert(AdoAdapter adapter, string tableName, IDictionary<string, object> data, IDbTransaction transaction)
    {
      var table = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindTable(tableName);
      if (table == null) throw new SimpleDataException(String.Format("Table '{0}' not found", tableName));

      var insertData = data.Where(p => table.HasColumn(p.Key) && !table.FindColumn(p.Key).IsIdentity).ToDictionary();

      var insertColumns = insertData.Keys.Select(table.FindColumn).ToArray();

      var columnsSql = insertColumns.Select(s => s.QuotedName).Aggregate((agg, next) => String.Concat(agg, ",", next));
      var valuesSql = insertColumns.Select((val, idx) => "@p" + idx.ToString()).Aggregate((agg, next) => String.Concat(agg, ",", next));

      var insertSql = string.Format("INSERT INTO {0} ({1}) VALUES({2}) RETURNING *;", table.QualifiedName, columnsSql, valuesSql);
      if (transaction != null)
      {
        var cmd = transaction.Connection.CreateCommand();
        cmd.Transaction = transaction;

        cmd.CommandText = insertSql;
        CreateParameters(cmd, insertColumns, insertData.Values.ToArray());

        return TryExecuteSingletonQuery(cmd);
      }

      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        
        cmd.CommandText = insertSql;
        CreateParameters(cmd, insertColumns, insertData.Values.ToArray());

        return TryExecuteSingletonQuery(cmd);
      }
    }

    private void CreateParameters(IDbCommand cmd, Column[] insertColumns, object[] insertData)
    {
      for (var idx = 0 ; idx < insertColumns.Length ; idx++)
      {
        var parameter = new NpgsqlParameter(String.Concat("@p", idx.ToString()), ((PgColumn)insertColumns[idx]).NpgsqlDbType);
        parameter.Value = insertData[idx];
        cmd.Parameters.Add(parameter);
      }
    }

    private IDictionary<string, object> TryExecuteSingletonQuery(IDbCommand cmd)
    {
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
  }
}