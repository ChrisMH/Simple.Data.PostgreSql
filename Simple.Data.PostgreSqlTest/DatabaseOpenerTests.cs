using NUnit.Framework;

namespace Simple.Data.PostgreSqlTest
{
  [TestFixture]
  public class DatabaseOpenerTests
  {
    [SetUp]
    public void SetUp()
    {
      DatabaseUtility.CreateDatabase();
    }

    [TearDown]
    public void TearDown()
    {
      //DatabaseUtility.DestroyDatabase();
    }

    [Test]
    public void OpenNamedConnectionTest()
    {
      var db = Database.OpenNamedConnection("Test");
      Assert.IsNotNull(db);
      var user = db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestProviderIsSqlProvider()
    {
      //var provider = new ProviderHelper().GetProviderByConnectionString(Properties.Settings.Default.ConnectionString);
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), provider);
      Assert.True(false);
    }

    [Test]
    public void TestProviderIsSqlProviderFromOpen()
    {
      //Database db = DatabaseHelper.Open();
      //Assert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
      Assert.True(false);
    }

    [Test]
    public void TestProviderIsSqlProviderFromOpenConnection()
    {
      //Database db = Database.OpenConnection(Properties.Settings.Default.ConnectionString);
      //Assert.IsInstanceOf(typeof(AdoAdapter), db.GetAdapter());
      //Assert.IsInstanceOf(typeof(SqlConnectionProvider), ((AdoAdapter)db.GetAdapter()).ConnectionProvider);
      Assert.True(false);
    }
  }
}
