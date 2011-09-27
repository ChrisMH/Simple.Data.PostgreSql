using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Simple.Data.PostgreSqlTest
{
  public class FindTests
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
    public void TestFindById()
    {
      var db = Database.Open();
      var user = db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestFindByIdWithCast()
    {
      var db = Database.Open();
      var user = (User)db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestFindByReturnsOne()
    {
      var db = Database.Open();
      var user = (User)db.Users.FindByName("Bob");
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestFindAllByName()
    {
      var db = Database.Open();
      IEnumerable<User> users = db.Users.FindAllByName("Bob").Cast<User>();
      Assert.AreEqual(1, users.Count());
    }

    [Test]
    public void TestFindAllByNameAsIEnumerableOfDynamic()
    {
      var db = Database.Open();
      IEnumerable<dynamic> users = db.Users.FindAllByName("Bob");
      Assert.AreEqual(1, users.Count());
    }

    [Test]
    public void TestFindAllByPartialName()
    {
      var db = Database.Open();
      IEnumerable<User> users = db.Users.FindAll(db.Users.Name.Like("Bob")).ToList<User>();
      Assert.AreEqual(1, users.Count());
    }

    [Test]
    public void TestAllCount()
    {
      var db = Database.Open();
      var count = db.Users.All().ToList().Count;
      Assert.AreEqual(3, count);
    }

    /*
    [Test]
    public void TestAllWithSkipCount()
    {
      var db = Database.Open();
      var count = db.Users.All().Skip(1).ToList().Count;
      Assert.AreEqual(2, count);
    }
    */

    [Test]
    public void TestImplicitCast()
    {
      var db = Database.Open();
      User user = db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestImplicitEnumerableCast()
    {
      var db = Database.Open();
      foreach (User user in db.Users.All())
      {
        Assert.IsNotNull(user);
      }
    }

    [Test]
    public void TestFindWithSchemaQualification()
    {
      var db = Database.Open();
            
      var publicActual = db.Public.SchemaTable.FindById(1);
      var testActual = db.Test.SchemaTable.FindById(1);

      Assert.IsNotNull(publicActual);
      Assert.AreEqual("Pass", publicActual.Description);
      Assert.IsNotNull(testActual);
      Assert.AreEqual("Pass", testActual.Description);
    }

    [Test]
    public void TestFindWithCriteriaAndSchemaQualification()
    {
      var db = Database.Open();

      var publicActual = db.Public.SchemaTable.Find(db.Public.SchemaTable.Id == 1);

      Assert.IsNotNull(publicActual);
      Assert.AreEqual("Pass", publicActual.Description);
    }

    [Test]
    public void TestFindOnAView()
    {
      var db = Database.Open();
      var u = db.ViewCustomers.FindById(1);
      Assert.IsNotNull(u);
    }

    [Test]
    public void TestCast()
    {
      var db = Database.Open();
      var userQuery = db.Users.All().Cast<User>() as IEnumerable<User>;
      Assert.IsNotNull(userQuery);
      var users = userQuery.ToList();
      Assert.AreNotEqual(0, users.Count);
    }
  }
}