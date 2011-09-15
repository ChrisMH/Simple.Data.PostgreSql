using System;
using System.ComponentModel.Composition;
using System.Data;
using Npgsql;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  [Export(typeof(IConnectionProvider))]
  [Export("Npgsql", typeof(IConnectionProvider))]
  public class PostgreSqlConnectionProvider : IConnectionProvider
  {
    public void SetConnectionString(string connectionString)
    {
      ConnectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
      return new NpgsqlConnection(ConnectionString);
    }

    public ISchemaProvider GetSchemaProvider()
    {
      return new PostgreSqlSchemaProvider(this);
    }

    public string ConnectionString { get; private set; }

    public bool SupportsCompoundStatements
    {
      get
      {
        // TODO: Implement this property getter
        throw new NotImplementedException();
      }
    }

    public string GetIdentityFunction()
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }

    public bool SupportsStoredProcedures
    {
      get
      {
        // TODO: Implement this property getter
        throw new NotImplementedException();
      }
    }

    public IProcedureExecutor GetProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
    {
      // TODO: Implement this method
      throw new NotImplementedException();
    }
  }
}
