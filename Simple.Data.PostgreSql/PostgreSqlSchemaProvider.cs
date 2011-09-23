using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Npgsql;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  internal class PostgreSqlSchemaProvider : ISchemaProvider
  {
    public PostgreSqlSchemaProvider(IConnectionProvider connectionProvider)
    {
      ConnectionProvider = connectionProvider;
    }

    public IConnectionProvider ConnectionProvider { get; private set; }

    public IEnumerable<Table> GetTables()
    {
      return SelectToDataTable("SELECT table_name, table_schema, table_type FROM information_schema.tables")
        .AsEnumerable()
        .Select(table => new Table(table["table_name"].ToString(),
                                   table["table_schema"].ToString(),
                                   table["table_type"].ToString().Equals("VIEW", StringComparison.InvariantCultureIgnoreCase) ? TableType.View : TableType.Table));
    }

    public IEnumerable<Column> GetColumns(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");
      
      return SelectToDataTable(String.Format(Properties.Resource.ColumnsQuery, table.Schema, table.ActualName))
        .AsEnumerable()
        .Select(column => new NpgsqlColumn(column["column_name"].ToString(),
                                           table,
                                           IsIdentityColumn(column["column_default"].ToString(), column["is_nullable"].ToString(), column["data_type"].ToString()),
                                           TypeResolver.GetDbType(column["data_type"].ToString()),
                                           Convert.IsDBNull(column["character_maximum_length"]) ? -1 : Convert.ToInt32(column["character_maximum_length"])));
    }

    public IEnumerable<Procedure> GetStoredProcedures()
    {
      return SelectToDataTable("SELECT routine_name, specific_name, routine_schema FROM information_schema.routines")
        .AsEnumerable()
        .Select(proc => new Procedure(proc["routine_name"].ToString(), proc["specific_name"].ToString(), proc["routine_schema"].ToString()));
    }

    public IEnumerable<Parameter> GetParameters(Procedure storedProcedure)
    {
      if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

      // TODO: This isn't going to work for overloaded stored procedures 
      // Npgsql uses the pg_proc table to lookup names.  The pg_proc table doesn't contain the specific (i.e., overloaded)
      // name.  Will probably have to use the information schema directly.
      using (var conn = ConnectionProvider.CreateConnection())
      {
        conn.Open();
        using (var cmd = (NpgsqlCommand) conn.CreateCommand())
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandText = storedProcedure.Name;

          NpgsqlCommandBuilder.DeriveParameters(cmd);

          foreach (NpgsqlParameter parameter in cmd.Parameters)
          {
            yield return new Parameter(parameter.ParameterName, TypeResolver.GetClrType(parameter.NpgsqlDbType.ToString()), parameter.Direction);
          }
        }
      }
    }

    public Key GetPrimaryKey(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      return new Key(SelectToDataTable(String.Format(Properties.Resource.PrimaryKeyQuery,table.Schema, table.ActualName))
                       .AsEnumerable()
                       .Select(column => column["column_name"].ToString()));
    }

    public IEnumerable<ForeignKey> GetForeignKeys(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      return SelectToDataTable(String.Format(Properties.Resource.ForeignKeysQuery, table.Schema, table.ActualName))
        .AsEnumerable()
        .GroupBy(s => s["constraint_name"].ToString())
        .Select(dataRows => new ForeignKey(new ObjectName(table.Schema, table.ActualName),
                                           dataRows.AsEnumerable().Select(r => r["column_name"].ToString()),
                                           new ObjectName(dataRows.First()["master_table_schema"].ToString(), dataRows.First()["master_table_name"].ToString()),
                                           dataRows.AsEnumerable().Select(r => r["master_column_name"].ToString())));
    }

    public string QuoteObjectName(string unquotedName)
    {
      if (String.IsNullOrEmpty(unquotedName)) throw new ArgumentNullException("unquotedName");
      return unquotedName.StartsWith("\"") ? unquotedName : String.Format("\"{0}\"", unquotedName);
    }

    public string NameParameter(string baseName)
    {
      if (String.IsNullOrEmpty(baseName)) throw new ArgumentNullException("baseName");
      return baseName.StartsWith("@") ? baseName : "@" + baseName;
    }

    private bool IsIdentityColumn(string columnDefault, string isNullable, string dataType)
    {
      if (String.IsNullOrEmpty(columnDefault) || !columnDefault.ToLowerInvariant().Contains("nextval"))
      {
        return false;
      }

      if (isNullable.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
      {
        return false;
      }

      if (!dataType.Equals("integer", StringComparison.InvariantCultureIgnoreCase) && !dataType.Equals("bigint", StringComparison.InvariantCultureIgnoreCase))
      {
        return false;
      }

      return true;
    }

    private DataTable SelectToDataTable(string sql)
    {
      var dataTable = new DataTable();
      using (var conn = ConnectionProvider.CreateConnection() as NpgsqlConnection)
      {
        using (var adapter = new NpgsqlDataAdapter(sql, conn))
        {
          adapter.Fill(dataTable);
        }
      }

      return dataTable;
    }
  }
}