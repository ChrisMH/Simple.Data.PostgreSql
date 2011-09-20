using System;
using System.Configuration;
using Simple.Data.Ado;
using Simple.Data.PostgreSql;
using Xunit;
using Simple.Data.PostgreSql;

namespace Simple.Data.PostgreSqlTest
{
  public class DatabaseOpenerTests : IDisposable
  {
    public DatabaseOpenerTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void OpenDefaultConnectionTest()
    {
      //var db = Database.Open();
      //Assert.NotNull(db);
      //var user = db.Public.Users.FindById(1);
      //Assert.Equal(1, user.Id);
    }

    [Fact]
    public void OpenNamedConnectionTest()
    {
      var db = Database.OpenNamedConnection("Test");
      Assert.NotNull(db);
      var user = db.Public.Users.FindById(1);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestProviderIsSqlProvider()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      Assert.True(provider is PostgreSqlConnectionProvider);
    }

    [Fact]
    public void TestProviderIsSqlProviderFromOpen()
    {
      var db = Database.Open();
      Assert.True(db.GetAdapter() is AdoAdapter);
      Assert.True(((AdoAdapter)db.GetAdapter()).ConnectionProvider is PostgreSqlConnectionProvider);
    }

    [Fact]
    public void TestProviderIsSqlProviderFromOpenConnection()
    {
      var db = Database.OpenConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      Assert.True(db.GetAdapter() is AdoAdapter);
      Assert.True(((AdoAdapter)db.GetAdapter()).ConnectionProvider is PostgreSqlConnectionProvider);
    }
  }
}
