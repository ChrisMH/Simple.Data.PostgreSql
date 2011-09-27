using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Npgsql;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using ResultSet = System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>;

namespace Simple.Data.PostgreSql
{
  public class PgProcedureExecutor : IProcedureExecutor
  {
    public PgProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
    {
      this.adapter = adapter;
      executeImpl = ExecuteReader;
      
      procedure = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindProcedure(procedureName);
      if (procedure == null)
      {
        throw new UnresolvableObjectException(procedureName.ToString());
      }

    }

    public IEnumerable<ResultSet> Execute(IDictionary<string, object> suppliedParameters)
    {
      // TODO: PostgreSql supports stored procedure overloading.  This does not.
      
      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandText = procedure.QualifiedName;
          cmd.CommandType = CommandType.StoredProcedure;
          AddCommandParameters(procedure, cmd, suppliedParameters);
          try
          {
            var result = executeImpl(cmd);
            GetReturnValue(result, suppliedParameters);

            return result;
          }
          catch (DbException ex)
          {
            throw new AdoAdapterException(ex.Message, cmd);
          }
        }
      }
    }

    private void GetReturnValue(IEnumerable<ResultSet> result, IDictionary<string, object> suppliedParameters)
    {
      if(result.Count() == 1)
      {
        var resultSet = result.First();

      }
    }

    public IEnumerable<ResultSet> ExecuteReader(IDbCommand cmd)
    {
      cmd.WriteTrace();
      cmd.Connection.Open();
      using (var rdr = cmd.ExecuteReader())
      {
        if (rdr.FieldCount == 0)
        {
          // Don't call ExecuteReader for this function again.
          executeImpl = ExecuteNonQuery;
          return Enumerable.Empty<ResultSet>();
        }

        if (procedure.Parameters.Where(param => param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output).Count() == 0)
        {
          // No output parameters
          if (rdr.FieldCount == 1 && rdr.GetName(0) == procedure.Name)
          {
            // Single field matching the name of the function.  Simple return value.

          }
        }
        return null;
      }
    }

    private static IEnumerable<ResultSet> ExecuteNonQuery(IDbCommand cmd)
    {
      cmd.WriteTrace();
      Trace.TraceInformation("ExecuteNonQuery", "Simple.Data.PostgreSql");
      cmd.Connection.Open();
      cmd.ExecuteNonQuery();
      return Enumerable.Empty<ResultSet>();
    }

    private static void AddCommandParameters(Procedure procedure, IDbCommand cmd, IDictionary<string, object> suppliedParameters)
    {
      int i = 0;
      foreach (var parameter in procedure.Parameters.Where(param => param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput))
      {
        object value;
        if(!suppliedParameters.TryGetValue(parameter.Name, out value))
        {
          if(!suppliedParameters.TryGetValue("_" + i, out value))
          {
            throw new SimpleDataException(String.Format("Could not find a value for parameter ordinal {0} named {1}", i, parameter.Name));
          }
        }

        cmd.Parameters.Add(new NpgsqlParameter
                             {
                               Value = value
                             });
        i++;
      }
    }

    
    private AdoAdapter adapter;
    private Procedure procedure;
    private Func<IDbCommand, IEnumerable<ResultSet>> executeImpl;
  }
}