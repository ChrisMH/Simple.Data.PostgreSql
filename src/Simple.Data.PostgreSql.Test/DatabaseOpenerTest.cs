using System.Configuration;
using NUnit.Framework;
using Simple.Data.Ado;

namespace Simple.Data.PostgreSql.Test
{
  public class DatabaseOpenerTest
  {
    [SetUp]
    public void SetUp()
    {
      GlobalTest.Database.Seed();
    }

    [Test]
    public void OpenDefaultConnectionTest()
    {
      var db = Database.Open();
      Assert.IsNotNull(db);
      var user = db.Public.Users.FindByName("Bob");
      Assert.True(user.id >= 0);
    }

    [Test]
    public void OpenNamedConnectionTest()
    {
      var db = Database.OpenNamedConnection("Test");
      Assert.IsNotNull(db);
      var user = db.Public.Users.FindByName("Bob");
      Assert.True(user.id >= 0);
    }

    [Test]
    public void TestProviderIsSqlProvider()
    {
      var provider = new ProviderHelper().GetProviderByConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      Assert.True(provider is PgConnectionProvider);
    }

    [Test]
    public void TestProviderIsSqlProviderFromOpen()
    {
      var db = Database.Open();
      Assert.True(db.GetAdapter() is AdoAdapter);
      Assert.True(((AdoAdapter) db.GetAdapter()).ConnectionProvider is PgConnectionProvider);
    }

    [Test]
    public void TestProviderIsSqlProviderFromOpenConnection()
    {
      var db = Database.OpenConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
      Assert.True(db.GetAdapter() is AdoAdapter);
      Assert.True(((AdoAdapter) db.GetAdapter()).ConnectionProvider is PgConnectionProvider);
    }
  }
}