using System;
using Xunit;

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
      //DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void OpenNamedConnectionTest()
    {
      var db = Database.OpenNamedConnection("Test");
      Assert.NotNull(db);
      var user = db.Users.FindById(1);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestProviderIsSqlProvider()
    {
      //var provider = new ProviderHelper().GetProviderByConnectionString(Properties.Settings.Default.ConnectionString);
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), provider);
      Assert.True(false);
    }

    [Fact]
    public void TestProviderIsSqlProviderFromOpen()
    {
      //Database db = DatabaseHelper.Open();
      //Assert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
      Assert.True(false);
    }

    [Fact]
    public void TestProviderIsSqlProviderFromOpenConnection()
    {
      //Database db = Database.OpenConnection(Properties.Settings.Default.ConnectionString);
      //Assert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
      Assert.True(false);
    }
  }
}
