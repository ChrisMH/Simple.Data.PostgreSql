using System;
using System.ComponentModel.Composition;
using System.Data;

using Npgsql;

using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Npgsql
{
  [Export(typeof(IConnectionProvider))]
  [Export("Npgsql", typeof(IConnectionProvider))]
  public class PgConnectionProvider : IConnectionProvider
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
      return new PgSchemaProvider(this);
    }

    public string ConnectionString { get; private set; }

    public bool SupportsCompoundStatements
    {
      get { return true; }
    }

    public string GetIdentityFunction()
    {
      // There is no global identity function in PostgreSql
      throw new InvalidOperationException("PostgreSql does not have a global identity function");
    }

    public bool SupportsStoredProcedures
    {
      get { return true; }
    }

    public IProcedureExecutor GetProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
    {
      return new PgProcedureExecutor(adapter, procedureName);
    }
  }
}