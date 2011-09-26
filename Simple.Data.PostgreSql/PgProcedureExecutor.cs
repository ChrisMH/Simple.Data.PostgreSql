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
      this.procedureName = procedureName;
      this.executeImpl = ExecuteReader;
    }

    public IEnumerable<IEnumerable<IDictionary<string, object>>> Execute(IDictionary<string, object> suppliedParameters)
    {
      var procedure = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindProcedure(procedureName);
      if (procedure == null)
      {
        throw new UnresolvableObjectException(procedureName.ToString());
      }
      
      using (var conn = adapter.ConnectionProvider.CreateConnection())
      {
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandText = procedure.QualifiedName;
          cmd.CommandType = CommandType.StoredProcedure;
          SetParameters(procedure, cmd, suppliedParameters);
          try
          {
            var result = executeImpl(cmd);
            if (cmd.Parameters.Contains(SimpleReturnParameterName))
              suppliedParameters[SimpleReturnParameterName] = GetParameterValue(cmd.Parameters, SimpleReturnParameterName);
            RetrieveOutputParameterValues(procedure, cmd, suppliedParameters);
            return result;
          }
          catch (DbException ex)
          {
            throw new AdoAdapterException(ex.Message, cmd);
          }
        }
      }
    }

    public IEnumerable<IEnumerable<IDictionary<string, object>>> ExecuteReader(IDbCommand command)
    {
      command.WriteTrace();
      command.Connection.Open();
      using (var reader = command.ExecuteReader())
      {
        if (reader.FieldCount > 0)
        {
          return reader.ToMultipleDictionaries();
        }

        // Don't call ExecuteReader for this function again.
        executeImpl = ExecuteNonQuery;
        return Enumerable.Empty<ResultSet>();
      }
    }


    private static void RetrieveOutputParameterValues(Procedure procedure, IDbCommand command, IDictionary<string, object> suppliedParameters)
    {
      foreach (var outputParameter in procedure.Parameters.Where(p => p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output))
      {
        suppliedParameters[outputParameter.Name.Replace("@", "")] = GetParameterValue(command.Parameters, outputParameter.Name);
      }
    }

    private static IEnumerable<ResultSet> ExecuteNonQuery(IDbCommand command)
    {
      command.WriteTrace();
      Trace.TraceInformation("ExecuteNonQuery", "Simple.Data.PostgreSql");
      command.Connection.Open();
      command.ExecuteNonQuery();
      return Enumerable.Empty<ResultSet>();
    }

    private static void SetParameters(Procedure procedure, IDbCommand cmd, IDictionary<string, object> suppliedParameters)
    {
      if (procedure.Parameters.Any(p => p.Direction == ParameterDirection.ReturnValue))
        AddReturnParameter(cmd);

      int i = 0;
      foreach (var parameter in procedure.Parameters.Where(p => p.Direction != ParameterDirection.ReturnValue))
      {
        object value;
        if (!suppliedParameters.TryGetValue(parameter.Name.Replace("@", ""), out value))
        {
          suppliedParameters.TryGetValue("_" + i, out value);
        }
        var cmdParameter = new NpgsqlParameter(parameter.Name, parameter.Type);
        cmdParameter.Value = value;
        cmdParameter.Direction = parameter.Direction;
        i++;
      }
    }

    private static void AddReturnParameter(IDbCommand cmd)
    {
      var returnParameter = cmd.CreateParameter();
      returnParameter.ParameterName = SimpleReturnParameterName;
      returnParameter.Direction = ParameterDirection.ReturnValue;
      cmd.Parameters.Add(returnParameter);
    }

    public static object GetParameterValue(IDataParameterCollection parameterCollection, string parameterName)
    {
      var parameter = parameterCollection[parameterName] as DbParameter;
      return parameter != null ? parameter.Value : null;
    }

    private const string SimpleReturnParameterName = "@__Simple_ReturnValue";
    private AdoAdapter adapter;
    private ObjectName procedureName;
    private Func<IDbCommand, IEnumerable<ResultSet>> executeImpl;
  }
}