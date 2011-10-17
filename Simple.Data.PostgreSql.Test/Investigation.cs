using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Npgsql;
using NpgsqlTypes;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class Investigation
  {
    [SetUp]
    public void SetUp()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    [TearDown]
    public void TearDown()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Test]
    public void TestReturn()
    {

      var parameters = GetParameters("public", "test_return");

      using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString))
      {
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandText = "public.test_return";



          cmd.Parameters.Add(new NpgsqlParameter
          {
            Direction = ParameterDirection.Input,
            Value = 2
          });
          cmd.Parameters.Add(new NpgsqlParameter
                               {
                                 ParameterName = "__return",
                                 Direction = ParameterDirection.ReturnValue
                               });



          using (var rdr = cmd.ExecuteReader())
          {
            rdr.Read();
          }
        }
      }
    }

    [Test]
    public void QueryBasicTypes()
    {
      using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString))
      {
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandText = "SELECT * FROM public.basic_types";
          using (var rdr = cmd.ExecuteReader())
          {
            while (rdr.Read())
            {
              Console.WriteLine("Field Name;Pg Type Name;DbType;NpgsqlDbType;Type;Value");
              var values = rdr.GetValues().ToArray();
              for (var field = 0; field < values.Length; field++)
              {
                Console.WriteLine("{0};{1};{2};{3};{4};{5}", rdr.GetName(field), rdr.GetDataTypeName(field), rdr.GetFieldDbType(field), rdr.GetFieldNpgsqlDbType(field), values[field].GetType(), values[field]);
              }
              Console.WriteLine();
            }
          }
        }
      }
    }

    [Test]
    public void QueryArrayTypes()
    {
      using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString))
      {
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandText = "SELECT * FROM public.array_types";
          using (var rdr = cmd.ExecuteReader())
          {
            while (rdr.Read())
            {
              Console.WriteLine("Field Name;Pg Type Name;DbType;NpgsqlDbType;Type;Value");
              var values = rdr.GetValues().ToArray();
              for (var field = 0; field < values.Length; field++)
              {
                Console.WriteLine("{0};{1};{2};{3};{4};{5}", rdr.GetName(field), rdr.GetDataTypeName(field), rdr.GetFieldDbType(field), rdr.GetFieldNpgsqlDbType(field), values[field].GetType(), values[field]);
              }
              Console.WriteLine();
            }
          }
        }
      }
    }


    private void FigureOutFunctionReturn(IEnumerable<Parameter> parameters, NpgsqlDataReader rdr, string actualName)
    {
      if (parameters.Where(param => param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output).Count() == 0)
      {
        if (rdr.FieldCount == 1 && rdr.GetName(0) == actualName)
        {
          // Simple return 
          rdr.Read();
          Console.WriteLine("Simple Return: {0}", rdr.GetValue(0));
        }
      }
    }

    private IEnumerable<Parameter> GetParameters(string schema, string name)
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schemaProvider = provider.GetSchemaProvider();
      return schemaProvider.GetParameters(schemaProvider.GetStoredProcedures().Where(p => p.Schema == schema && p.Name == name).First());
    }
  }
}