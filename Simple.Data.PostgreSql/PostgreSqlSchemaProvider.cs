using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  class PostgreSqlSchemaProvider : ISchemaProvider
  {
    public PostgreSqlSchemaProvider(IConnectionProvider connectionProvider)
    {
      ConnectionProvider = connectionProvider;
    }

    public IConnectionProvider ConnectionProvider { get; private set; }

    public IEnumerable<Table> GetTables()
    {
      using(var conn = ConnectionProvider.CreateConnection())
      {
        conn.Open();
        var schema = conn.GetSchema("Tables").AsEnumerable().Select(SchemaRowToTable);
        return schema;
      }
    }

    private static Table SchemaRowToTable(DataRow row)
    {
        return new Table(row["TABLE_NAME"].ToString(), row["TABLE_SCHEMA"].ToString(),
                    row["TABLE_TYPE"].ToString() == "BASE TABLE" ? TableType.Table : TableType.View);
    }

    public IEnumerable<Column> GetColumns(Table table)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
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
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public string NameParameter(string baseName)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }
  }
}
