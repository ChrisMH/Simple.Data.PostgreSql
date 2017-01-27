using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Npgsql;

using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.Npgsql
{
  internal class PgSchemaProvider : ISchemaProvider
  {
    public PgSchemaProvider(IConnectionProvider connectionProvider)
    {
      ConnectionProvider = connectionProvider;
    }

    public IConnectionProvider ConnectionProvider { get; private set; }

    public IEnumerable<Table> GetTables()
    {
      return SelectToDataTable(Resources.TablesQuery)
        .AsEnumerable()
        .Select(table => new Table(table["table_name"].ToString(),
                                   table["table_schema"].ToString(),
                                   table["table_type"].ToString().Equals("VIEW", StringComparison.InvariantCultureIgnoreCase) ? TableType.View : TableType.Table));
    }

    public IEnumerable<Column> GetColumns(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      var columns = SelectToDataTable(String.Format(Resources.ColumnsQuery, table.Schema, table.ActualName)).AsEnumerable();

      var foundIdentity = false;
      foreach (var column in columns)
      {
        var isIdentity = !foundIdentity && IsIdentityColumn(column["column_default"].ToString(), column["is_nullable"].ToString(), column["data_type"].ToString());
        if (isIdentity)
        {
          foundIdentity = true;
        }

        yield return new Column(column["column_name"].ToString(),
                                  table,
                                  isIdentity,
                                  TypeMap.GetTypeEntry(column["data_type"].ToString()).DbType,
                                  Convert.IsDBNull(column["maximum_length"]) ? -1 : Convert.ToInt32(column["maximum_length"]));
      }
    }

    public IEnumerable<Procedure> GetStoredProcedures()
    {
      return SelectToDataTable(Resources.StoredProceduresQuery)
        .AsEnumerable()
        .Select(proc => new Procedure(proc["routine_name"].ToString(), proc["specific_name"].ToString(), proc["routine_schema"].ToString()));
    }

    public IEnumerable<Parameter> GetParameters(Procedure storedProcedure)
    {
      if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

      return SelectToDataTable(String.Format(Resources.ParametersQuery, storedProcedure.Schema, storedProcedure.SpecificName))
        .AsEnumerable()
        .Select(row => new Parameter(Convert.IsDBNull(row["parameter_name"]) ? null : row["parameter_name"].ToString(),
                                       TypeMap.GetTypeEntry(row["data_type"].ToString()).ClrType,
                                       GetParameterDirection(row["parameter_mode"].ToString()))
                                       /*
                                       TypeMap.GetTypeEntry(row["data_type"].ToString()).DbType,
                                       Convert.IsDBNull(row["maximum_length"]) ? -1 : Convert.ToInt32(row["maximum_length"]))*/
        );
    }

    private ParameterDirection GetParameterDirection(string parameterMode)
    {
      switch (parameterMode)
      {
        case "IN":
          return ParameterDirection.Input;
        case "OUT":
          return ParameterDirection.Output;
        case "INOUT":
          return ParameterDirection.InputOutput;
        case "RETURN":
          return ParameterDirection.ReturnValue;
        default:
          throw new SimpleDataException(String.Format("Unknown parameter mode: {0}", parameterMode));
      }
    }

    public Key GetPrimaryKey(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      return new Key(SelectToDataTable(String.Format(Resources.PrimaryKeyQuery, table.Schema, table.ActualName))
                       .AsEnumerable()
                       .Select(column => column["column_name"].ToString()));
    }

    public IEnumerable<ForeignKey> GetForeignKeys(Table table)
    {
      if (table == null) throw new ArgumentNullException("table");

      return SelectToDataTable(String.Format(Resources.ForeignKeysQuery, table.Schema, table.ActualName))
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
      return baseName.StartsWith(":") ? baseName : String.Concat(":", baseName);
    }

    public string GetDefaultSchema()
    {
      return "public";
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

    private static bool IsIdentityColumn(string columnDefault, string isNullable, string dataType)
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
  }
}