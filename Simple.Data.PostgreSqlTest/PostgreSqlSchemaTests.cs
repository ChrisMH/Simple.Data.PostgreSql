using System.Configuration;
using System.Data;
using NpgsqlTypes;
using Simple.Data.Ado;
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
    public void CanGetTables()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var result = schema.GetTables().Where(s => s.ActualName == "users").SingleOrDefault();

      Assert.NotNull(result);
    }

    [Fact]
    public void CanGetColumns()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      var schema = provider.GetSchemaProvider();

      var table = schema.GetTables().Where(s => s.ActualName == "users").Single();

      var column = (NpgsqlColumn)schema.GetColumns(table).Where(p => p.ActualName == "id").Single();
      Assert.True(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Integer, column.NpgsqlDbType);

      column = (NpgsqlColumn)schema.GetColumns(table).Where(p => p.ActualName == "name").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (NpgsqlColumn)schema.GetColumns(table).Where(p => p.ActualName == "password").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Varchar, column.NpgsqlDbType);

      column = (NpgsqlColumn)schema.GetColumns(table).Where(p => p.ActualName == "age").Single();
      Assert.False(column.IsIdentity);
      Assert.Equal(NpgsqlDbType.Integer, column.NpgsqlDbType);
    }
  }
}