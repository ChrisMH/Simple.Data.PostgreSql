using System;
using System.Configuration;
using System.Data;
using NUnit.Framework;
using NpgsqlTypes;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.PostgreSql;
using System.Linq;

namespace Simple.Data.PostgreSqlTest
{
  public class PostgreSqlSchemaTests
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
    public void CanGetTable()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetTables().Where(s => s.ActualName == "users").SingleOrDefault();

      Assert.IsNotNull(result);
      Assert.AreEqual(TableType.Table, result.Type);
    }

    [Test]
    public void CanGetView()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetTables().Where(s => s.ActualName == "view_customers").SingleOrDefault();
      Assert.IsNotNull(result);
      Assert.AreEqual(TableType.View, result.Type);
    }

    [Test]
    public void CanGetStoredProcedure()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetStoredProcedures().Where(s => s.Name == "get_customers").SingleOrDefault();
      Assert.IsNotNull(result);
    }


    [Test]
    public void CanGetTableColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "users").Single();

      var column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.True(column.IsIdentity);
      Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "password").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "age").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }

    [Test]
    public void CanGetViewColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "view_customers").Single();

      var column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "address").Single();
      Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (PgColumn) schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }

    [Test]
    public void CanGetStoredProcedureParameters()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var proc = schema.GetStoredProcedures().Where(s => s.Name == "get_customer_orders").SingleOrDefault();
      var result = schema.GetParameters(proc);

      Assert.AreEqual(1, result.Count());
      Assert.AreEqual(typeof (int), result.First().Type);
    }


    [Test]
    public void CanGetPrimaryKey()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "order_items").SingleOrDefault();
      var result = schema.GetPrimaryKey(table);

      Assert.IsNotNull(result);
      Assert.AreEqual(1, result.Length);
      Assert.AreEqual("id", result[0]);
    }

    [Test]
    public void CanGetPrimaryKeyWhenTableHasNoPrimaryKey()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "no_primary_key_test").SingleOrDefault();
      var result = schema.GetPrimaryKey(table);

      Assert.IsNotNull(result);
      Assert.AreEqual(0, result.Length);
    }


    [Test]
    public void CanGetForeignKeys()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "order_items").SingleOrDefault();
      var result = schema.GetForeignKeys(table).ToList();

      Assert.IsNotNull(result);
      Assert.AreEqual(2, result.Count());
      
      var fk = result.Where(p => p.MasterTable.Name == "orders").FirstOrDefault();
      Assert.IsNotNull(fk);
      Assert.AreEqual("public", fk.DetailTable.Schema);
      Assert.AreEqual("order_items", fk.DetailTable.Name);
      Assert.AreEqual(1, fk.Columns.Length);
      Assert.AreEqual("order_id", fk.Columns[0]);

      Assert.AreEqual("public", fk.MasterTable.Schema);
      Assert.AreEqual("orders", fk.MasterTable.Name);
      Assert.AreEqual(1, fk.UniqueColumns.Length);
      Assert.AreEqual("id", fk.UniqueColumns[0]);

      fk = result.Where(p => p.MasterTable.Name == "items").FirstOrDefault();
      Assert.IsNotNull(fk);
      Assert.AreEqual("public", fk.DetailTable.Schema);
      Assert.AreEqual("order_items", fk.DetailTable.Name);
      Assert.AreEqual(1, fk.Columns.Length);
      Assert.AreEqual("item_id", fk.Columns[0]);

      Assert.AreEqual("public", fk.MasterTable.Schema);
      Assert.AreEqual("items", fk.MasterTable.Name);
      Assert.AreEqual(1, fk.UniqueColumns.Length);
      Assert.AreEqual("id", fk.UniqueColumns[0]);
    }
  }
}