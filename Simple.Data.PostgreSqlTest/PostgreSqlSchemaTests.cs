using System;
using System.Configuration;
using System.Data;
using NpgsqlTypes;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.PostgreSql;
using Xunit;
using System.Linq;

namespace Simple.Data.PostgreSqlTest
{
  public class PostgreSqlSchemaTests
  {
    public PostgreSqlSchemaTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void CanGetTable()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetTables().Where(s => s.ActualName == "users").SingleOrDefault();

      Assert.NotNull(result);
      Assert.Equal(TableType.Table, result.Type);
    }

    [Fact]
    public void CanGetView()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetTables().Where(s => s.ActualName == "view_customers").SingleOrDefault();
      Assert.NotNull(result);
      Assert.Equal(TableType.View, result.Type);
    }

    [Fact]
    public void CanGetStoredProcedure()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetStoredProcedures().Where(s => s.Name == "get_customers").SingleOrDefault();
      Assert.NotNull(result);
    }


    [Fact]
    public void CanGetTableColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "users").Single();

      var column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.True(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Integer, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "password").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "age").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }

    [Fact]
    public void CanGetViewColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "view_customers").Single();

      var column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "address").Single();
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.Equal(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }

    [Fact]
    public void CanGetStoredProcedureParameters()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var proc = schema.GetStoredProcedures().Where(s => s.Name == "get_customer_orders").SingleOrDefault();
      var result = schema.GetParameters(proc);

      Assert.Equal(1, result.Count());
      Assert.Equal(typeof (int), result.First().Type);
    }


    [Fact]
    public void CanGetPrimaryKey()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "order_items").SingleOrDefault();
      var result = schema.GetPrimaryKey(table);

      Assert.NotNull(result);
      Assert.Equal(1, result.Length);
      Assert.Equal("id", result[0]);
    }

    [Fact]
    public void CanGetPrimaryKeyWhenTableHasNoPrimaryKey()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "no_primary_key_test").SingleOrDefault();
      var result = schema.GetPrimaryKey(table);

      Assert.NotNull(result);
      Assert.Equal(0, result.Length);
    }


    [Fact]
    public void CanGetForeignKeys()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "order_items").SingleOrDefault();
      var result = schema.GetForeignKeys(table).ToList();

      Assert.NotNull(result);
      Assert.Equal(2, result.Count());
      
      var fk = result.Where(p => p.MasterTable.Name == "orders").FirstOrDefault();
      Assert.NotNull(fk);
      Assert.Equal("public", fk.DetailTable.Schema);
      Assert.Equal("order_items", fk.DetailTable.Name);
      Assert.Equal(1, fk.Columns.Length);
      Assert.Equal("order_id", fk.Columns[0]);

      Assert.Equal("public", fk.MasterTable.Schema);
      Assert.Equal("orders", fk.MasterTable.Name);
      Assert.Equal(1, fk.UniqueColumns.Length);
      Assert.Equal("id", fk.UniqueColumns[0]);

      fk = result.Where(p => p.MasterTable.Name == "items").FirstOrDefault();
      Assert.NotNull(fk);
      Assert.Equal("public", fk.DetailTable.Schema);
      Assert.Equal("order_items", fk.DetailTable.Name);
      Assert.Equal(1, fk.Columns.Length);
      Assert.Equal("item_id", fk.Columns[0]);

      Assert.Equal("public", fk.MasterTable.Schema);
      Assert.Equal("items", fk.MasterTable.Name);
      Assert.Equal(1, fk.UniqueColumns.Length);
      Assert.Equal("id", fk.UniqueColumns[0]);
    }
  }
}