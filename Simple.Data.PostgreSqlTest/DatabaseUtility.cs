using System;
using System.Configuration;
using System.Data.Common;

namespace Simple.Data.PostgreSqlTest
{
  public static class DatabaseUtility
  {

    public static void CreateDatabase()
    {
      DestroyDatabase();

      var connectionString = new DbConnectionStringBuilder { ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString };
      var database = connectionString["database"];
      connectionString["database"] = "postgres";

      var conn = new Npgsql.NpgsqlConnection(connectionString.ConnectionString);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = String.Format("CREATE DATABASE {0}", database);
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }

      connectionString["database"] = database;
      conn = new Npgsql.NpgsqlConnection(connectionString.ConnectionString);
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
    
    public static void DestroyDatabase()
    {     
      var connectionString = new DbConnectionStringBuilder { ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString };
      var database = connectionString["database"];
      connectionString["database"] = "postgres";

      var conn = new Npgsql.NpgsqlConnection(connectionString.ConnectionString);
      try
      {
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = String.Format("DROP DATABASE {0}", database);
        cmd.ExecuteNonQuery();
      }
      finally
      {
        conn.Close();
      }
    }
  }
}
