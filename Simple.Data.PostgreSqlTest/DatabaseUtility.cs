using System;
using System.Configuration;
using System.Data.Common;
using Npgsql;

namespace Simple.Data.PostgreSqlTest
{
  public static class DatabaseUtility
  {

    public static void CreateDatabase(string connectionStringName)
    {
      if(DatabaseExists(connectionStringName))
      {
        DestroyDatabase(connectionStringName);
      }

      var conn = GetSuperuserConnection(connectionStringName);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = String.Format("CREATE DATABASE {0}", GetConnectionString(connectionStringName)["database"]);
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }

      conn = GetConnection(connectionStringName);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"CREATE TABLE public.users (
                              id           serial NOT NULL,
                              ""name""     varchar(100) NOT NULL,
                              ""password"" varchar(100) NOT NULL,
                              age          integer NOT NULL,
                              CONSTRAINT users_pkey
                                PRIMARY KEY (id)
                            ) WITH (OIDS = FALSE)";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO public.users VALUES (1, 'Bob', 'Bob', 32)";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO public.users VALUES (2, 'Charlie', 'Charlie', 49)";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "INSERT INTO public.users VALUES (3, 'Dave', 'Dave', 12)";
        cmd.ExecuteNonQuery();
        cmd.CommandText = "ALTER SEQUENCE public.users_id_seq RESTART WITH 3";
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }
    }
    
    public static void DestroyDatabase(string connectionStringName)
    {     
      if(!DatabaseExists(connectionStringName))
      {
        return;
      }

      var conn = GetSuperuserConnection(connectionStringName);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = String.Format("DROP DATABASE {0}", GetConnectionString(connectionStringName)["database"]);
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }
    }

    public static bool DatabaseExists(string connectionStringName)
    {
      var conn = GetSuperuserConnection(connectionStringName);
      try
      {
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = string.Format("SELECT COUNT(*) FROM pg_database WHERE datname='{0}'", GetConnectionString(connectionStringName)["database"]);

        long count = Convert.ToInt64(cmd.ExecuteScalar());

        return count > 0;
      }
      finally
      {
        conn.Close();
      }
    }

    public static DbConnection GetConnection(string connectionStringName)
    {
      var connectionString = GetConnectionString(connectionStringName);

      return new NpgsqlConnection(connectionString.ConnectionString);
    }

    public static DbConnection GetSuperuserConnection(string connectionStringName)
    {
      var connectionString = GetConnectionString(connectionStringName);
      connectionString["database"] = "postgres";

      return new NpgsqlConnection(connectionString.ConnectionString);
    }

    public static DbConnectionStringBuilder GetConnectionString(string connectionStringName)
    {
      return new DbConnectionStringBuilder { ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString };
    }
  }
}
