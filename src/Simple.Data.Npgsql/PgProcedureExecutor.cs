using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Npgsql;

using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

using ResultSet = System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>;

namespace Simple.Data.Npgsql
{
  public class PgProcedureExecutor : IProcedureExecutor
  {
    public PgProcedureExecutor(AdoAdapter adapter, ObjectName procedureName)
    {
      Adapter = adapter;
      ProcedureName = procedureName;

    }

    public IEnumerable<ResultSet> Execute(IDictionary<string, object> suppliedParameters, IDbTransaction transaction)
    {
      return Execute(suppliedParameters, transaction.Connection);
    }

    public IEnumerable<ResultSet> Execute(IDictionary<string, object> suppliedParameters)
    {
      using (var conn = Adapter.ConnectionProvider.CreateConnection())
      {
        conn.Open();
        return Execute(suppliedParameters, conn);
      }
    }

    IEnumerable<ResultSet> Execute(IDictionary<string, object> suppliedParameters, IDbConnection conn)
    {
      // TODO: PostgreSql supports stored procedure overloading.  This does not.
      var procedure = DatabaseSchema.Get(Adapter.ConnectionProvider, Adapter.ProviderHelper).FindProcedure(ProcedureName);
      if (procedure == null)
      {
        throw new UnresolvableObjectException(ProcedureName.ToString());
      }

      var executeReader = procedure.Parameters.Any(p => p.Direction == ParameterDirection.InputOutput ||
                                                      p.Direction == ParameterDirection.Output ||
                                                      p.Direction == ParameterDirection.ReturnValue);

      using (var cmd = conn.CreateCommand())
      {
        cmd.CommandText = procedure.QualifiedName;
        cmd.CommandType = CommandType.StoredProcedure;
        AddCommandParameters(procedure, cmd, suppliedParameters);
        try
        {
          var result = Enumerable.Empty<ResultSet>();

          cmd.WriteTrace();
          if (executeReader)
          {
            using (var rdr = cmd.ExecuteReader())
            {
              var rdrAdvanced = RetrieveReturnValue(procedure, rdr, suppliedParameters);
              rdrAdvanced = RetrieveOutputParameterValues(procedure, rdr, suppliedParameters, rdrAdvanced);

              if (!rdrAdvanced)
              {
                result = rdr.ToMultipleDictionaries();
              }
            }
          }
          else
          {
            cmd.ExecuteNonQuery();
          }

          return result;
        }
        catch (DbException ex)
        {
          throw new AdoAdapterException(ex.Message, cmd);
        }
      }

    }

    public IEnumerable<ResultSet> ExecuteReader(IDbCommand cmd)
    {
      // Exposed in the IProcedureExecutor, but not called externally.  Don't use this.
      throw new NotImplementedException();
    }

    private void AddCommandParameters(Procedure procedure, IDbCommand cmd, IDictionary<string, object> suppliedParameters)
    {
      foreach (var parameter in procedure.Parameters
        .Where(param => param.Direction == ParameterDirection.Input ||
                        param.Direction == ParameterDirection.InputOutput)
        .Select((value, index) => new {Parameter = value, Index = index}))
      {
        var name = parameter.Parameter.Name;
        object value;

        if (String.IsNullOrEmpty(name) || !suppliedParameters.TryGetValue(name, out value))
        {
          name = String.Concat("_", parameter.Index);
          if (!suppliedParameters.TryGetValue(name, out value))
          {
            throw new SimpleDataException(String.Format("Could not find a value for parameter index {0} named {1}", parameter.Index, parameter.Parameter.Name));
          }
        }

        // No need to use parameter names here.  Position is what is important.
        cmd.Parameters.Add(new NpgsqlParameter
                             {
                               ParameterName = name,
                               Value = value
                             });
      }
    }

    private bool RetrieveReturnValue(Procedure procedure, IDataReader rdr, IDictionary<string, object> suppliedParameters)
    {
      // If there is areturn value its column name will be the function name
      var field = rdr.FindField(procedure.Name);
      if (field >= 0)
      {
        if (!rdr.Read())
        {
          throw new Exception("Empty IDataReader");
        }
        suppliedParameters["__ReturnValue"] = rdr[field];
        return true;
      }
      return false;
    }

    private bool RetrieveOutputParameterValues(Procedure procedure, IDataReader rdr, IDictionary<string, object> suppliedParameters, bool rdrAdvanced)
    {
      var index = 0;

      foreach (var parameter in procedure.Parameters)
      {
        if (parameter.Direction != ParameterDirection.InputOutput && parameter.Direction != ParameterDirection.Output)
        {
          continue;
        }

        if (!rdrAdvanced)
        {
          if (!rdr.Read())
          {
            throw new Exception("Empty IDataReader");
          }
          rdrAdvanced = true;
        }

        var name = parameter.Name ?? String.Concat("output", index);
        suppliedParameters[name] = rdr[index];
        index++;
      }

      return rdrAdvanced;
    }

    private AdoAdapter Adapter { get; set; }
    private ObjectName ProcedureName { get; set; }
  }

  internal static class DataReaderExtensions
  {
    internal static int FindField(this IDataReader rdr, string fieldName)
    {
      for (var i = 0; i < rdr.FieldCount; i++)
      {
        if (rdr.GetName(i).Equals(fieldName))
        {
          return i;
        }
      }
      return -1;
    }
  }
}