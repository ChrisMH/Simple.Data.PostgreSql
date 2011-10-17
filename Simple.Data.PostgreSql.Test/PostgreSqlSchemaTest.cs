using System;
using System.Configuration;
using System.Data;
using System.Linq;
using NUnit.Framework;
using NpgsqlTypes;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class PostgreSqlSchemaTest
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

      var result = schema.GetTables().Where(s => s.ActualName == "basic_types").SingleOrDefault();

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
    public void CanGetBasicTypesTableColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "basic_types").Single();

      var column =  schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.True(column.IsIdentity);
      Assert.AreEqual(DbType.Int32, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column =  schema.GetColumns(table).Where(p => p.ActualName == "smallint_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int16, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Smallint, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column =  schema.GetColumns(table).Where(p => p.ActualName == "integer_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int32, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column =  schema.GetColumns(table).Where(p => p.ActualName == "bigint_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int64, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bigint, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "decimal_unlimited_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Decimal, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Numeric, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "decimal_10_2_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Decimal, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Numeric, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "numeric_unlimited_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Decimal, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Numeric, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "numeric_10_2_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Decimal, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Numeric, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "real_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Single, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Real, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);
      
      column = schema.GetColumns(table).Where(p => p.ActualName == "double_precision_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Double, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Double, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "serial_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int32, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bigserial_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int64, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bigint, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "money_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Currency, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Money, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "character_varying_unlimited_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "character_varying_30_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);
      Assert.AreEqual(30, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "varchar_unlimited_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "varchar_30_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);
      Assert.AreEqual(30, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "character_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "character_10_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(10, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "char_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "char_10_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(10, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "text_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bytea_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Binary, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bytea, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "timestamp_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.DateTime, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Timestamp, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "timestamp_without_time_zone_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.DateTime, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Timestamp, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "timestamptz_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.DateTime, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.TimestampTZ, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "timestamp_with_time_zone_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.DateTime, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.TimestampTZ, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "date_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Date, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Date, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "time_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Time, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Time, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "time_without_time_zone_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Time, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Time, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "timetz_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Time, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.TimeTZ, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "time_with_time_zone_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Time, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.TimeTZ, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "interval_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Interval, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "boolean_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Boolean, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Boolean, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "point_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Point, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "lseg_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.LSeg, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "box_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Box, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "path_closed_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Path, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "path_open_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Path, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "polygon_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Polygon, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "circle_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Circle, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);
      
      column = schema.GetColumns(table).Where(p => p.ActualName == "cidr_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "inet_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Inet, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "macaddr_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bit_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bit, column.NpgsqlDbType);
      Assert.AreEqual(1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bit_10_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bit, column.NpgsqlDbType);
      Assert.AreEqual(10, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bit_varying_unlimited_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "bit_varying_10_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(10, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "tsvector_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "tsquery_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Text, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "uuid_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Guid, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Uuid, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "oid_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Int64, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Bigint, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

    }

    [Test]
    public void CanGetArrayTypesTableColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "array_types").Single();

      var column =  schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.True(column.IsIdentity);
      Assert.AreEqual(DbType.Int32, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "integer_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "real_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "double_precision_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "varchar_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "integer_multi_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "real_multi_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "double_multi_precision_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

      column = schema.GetColumns(table).Where(p => p.ActualName == "varchar_multi_array_field").Single();
      Assert.False(column.IsIdentity);
      Assert.AreEqual(DbType.Object, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
      Assert.AreEqual(-1, column.MaxLength);

    }


    [Test]
    public void CanGetViewColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "view_customers").Single();

      var column =  schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = schema.GetColumns(table).Where(p => p.ActualName == "address").Single();
      Assert.AreEqual(DbType.String, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.AreEqual(DbType.Int32, column.DbType);
      //Assert.AreEqual(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }

    [Test]
    public void CanGetStoredProcedureParameters()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var proc = schema.GetStoredProcedures().Where(s => s.Name == "test_return").SingleOrDefault();
      var result = schema.GetParameters(proc).ToArray();

      Assert.AreEqual(2, result.Count());
      Assert.AreEqual(PgSchemaProvider.ReturnParameterName, result[0].Name);
      Assert.AreEqual(typeof(int), result[0].Type);
      Assert.AreEqual(ParameterDirection.ReturnValue, result[0].Direction);

      Assert.AreEqual("double_me", result[1].Name);
      Assert.AreEqual(typeof(int), result[1].Type);
      Assert.AreEqual(ParameterDirection.Input, result[1].Direction);
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