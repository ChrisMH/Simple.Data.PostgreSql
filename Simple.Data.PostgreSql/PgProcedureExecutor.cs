using System.Collections.Generic;
using System.Data;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  public class PgProcedureExecutor : IProcedureExecutor
  {
    public PgProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
    {
      this.adapter = adapter;
      this.procedureName = procedureName;
    }

    public IEnumerable<IEnumerable<IDictionary<string, object>>> Execute(IDictionary<string, object> suppliedParameters)
    {
      var procedures = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindProcedure(procedureName);

      var parameters = procedures.Parameters;

      return null;
    }

    public IEnumerable<IEnumerable<IDictionary<string, object>>> ExecuteReader(IDbCommand command)
    {
      return null;
    }

    private AdoAdapter adapter;
    private ObjectName procedureName;
  }
}