using System;
using System.Collections.Generic;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  internal class PostgreSqlSchemaProvider : ISchemaProvider
  {
    public PostgreSqlSchemaProvider(IConnectionProvider connectionProvider)
    {
      ConnectionProvider = connectionProvider;
    }

    public IConnectionProvider ConnectionProvider { get; private set; }

    public IEnumerable<Table> GetTables()
    {
      using (var conn = ConnectionProvider.CreateConnection())
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT table_name, table_schema, table_type FROM information_schema.tables";
        using (var rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            yield return new Table(rdr.GetString(0), rdr.GetString(1),
                                   rdr.GetString(2).Equals("VIEW", StringComparison.InvariantCultureIgnoreCase)
                                     ? TableType.View
                                     : TableType.Table);
          }
        }
      }
    }

    public IEnumerable<Column> GetColumns(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      using (var conn = ConnectionProvider.CreateConnection())
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT column_name, column_default, is_nullable, data_type, character_maximum_length " +
                          "FROM information_schema.columns " +
                          String.Format("WHERE table_schema='{0}' AND table_name='{1}'", table.Schema, table.ActualName);
        using (var rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            yield return new Column(rdr.GetString(0),
                                    table,
                                    IsIdentityColumn(rdr.IsDBNull(1) ? null : rdr.GetString(1), rdr.GetString(2), rdr.GetString(3)),
                                    DbTypeLookup.GetDbType(rdr.GetString(3)),
                                    rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4));
          }
        }
      }
    }


    public IEnumerable<Procedure> GetStoredProcedures()
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public IEnumerable<Parameter> GetParameters(Procedure storedProcedure)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public Key GetPrimaryKey(Table table)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public IEnumerable<ForeignKey> GetForeignKeys(Table table)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public string QuoteObjectName(string unquotedName)
    {
      if (unquotedName.StartsWith("\""))
      {
        return unquotedName;
      }
      return String.Format("\"{0}\"", unquotedName);
    }

    public string NameParameter(string baseName)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    internal bool IsIdentityColumn(string columnDefault, string isNullable, string dataType)
    {
      if (String.IsNullOrEmpty(columnDefault)
          || !columnDefault.ToLowerInvariant().Contains("nextval"))
      {
        return false;
      }

      if (isNullable.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
      {
        return false;
      }

      if (!dataType.Equals("integer", StringComparison.InvariantCultureIgnoreCase) &&
          !dataType.Equals("bigint", StringComparison.InvariantCultureIgnoreCase))
      {
        return false;
      }

      return true;
    }
  }
}