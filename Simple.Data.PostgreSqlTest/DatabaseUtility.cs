using System;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Reflection;
using Npgsql;

namespace Simple.Data.PostgreSqlTest
{
  public static class DatabaseUtility
  {

    public static void CreateDatabase(string connectionStringName)
    {
      DestroyDatabase(connectionStringName);

      var connectionString = GetConnectionString(connectionStringName);
      var suConnectionString = GetSuperuserConnectionString(connectionStringName);

      using(var conn = new NpgsqlConnection(suConnectionString.ConnectionString))
      {
        conn.Open();
        var cmd = conn.CreateCommand();

        cmd.CommandText = String.Format("CREATE DATABASE {0}", connectionString["database"]);
        cmd.ExecuteNonQuery();

        cmd.CommandText = String.Format("CREATE ROLE {0} LOGIN ENCRYPTED PASSWORD '{1}' NOINHERIT",
                                        connectionString["user id"], connectionString["password"]);
        cmd.ExecuteNonQuery();
      }

      suConnectionString["database"] = connectionString["database"];
      using(var conn = new NpgsqlConnection(suConnectionString.ConnectionString))
      {
        conn.Open();
        var cmd = conn.CreateCommand();

        cmd.CommandText = string.Format("ALTER DEFAULT PRIVILEGES GRANT ALL ON TABLES TO {0}", connectionString["user id"]);
        cmd.ExecuteNonQuery();

        cmd.CommandText = string.Format("ALTER DEFAULT PRIVILEGES GRANT ALL ON SEQUENCES TO {0}", connectionString["user id"]);
        cmd.ExecuteNonQuery();

        cmd.CommandText = string.Format("ALTER DEFAULT PRIVILEGES GRANT ALL ON FUNCTIONS TO {0}", connectionString["user id"]);
        cmd.ExecuteNonQuery();

        var testSql = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Simple.Data.PostgreSqlTest.Resources.Test.sql"));
        cmd.CommandText = testSql.ReadToEnd();
        cmd.ExecuteNonQuery();
      }
    }
    
    public static void DestroyDatabase(string connectionStringName)
    {
      var connectionString = GetConnectionString(connectionStringName);
      var suConnectionString = GetSuperuserConnectionString(connectionStringName);

      var conn = new NpgsqlConnection(suConnectionString.ConnectionString);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();

        cmd.CommandText = String.Format("DROP DATABASE IF EXISTS {0}", connectionString["database"]);
        cmd.ExecuteNonQuery();

        cmd.CommandText = String.Format("DROP ROLE IF EXISTS {0}", connectionString["user id"]);
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }
    }
    
    public static DbConnectionStringBuilder GetConnectionString(string connectionStringName)
    {
      return new DbConnectionStringBuilder { ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString };
    }

    public static DbConnectionStringBuilder GetSuperuserConnectionString(string connectionStringName)
    {
      var suConnectionString = GetConnectionString(connectionStringName);
      suConnectionString["database"] = ConfigurationManager.AppSettings["superuserDatabase"];
      suConnectionString["user id"] = ConfigurationManager.AppSettings["superuserUserId"];
      suConnectionString["password"] = ConfigurationManager.AppSettings["superuserPassword"];
      return suConnectionString;
    }
  }
}
