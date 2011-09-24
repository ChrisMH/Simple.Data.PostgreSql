using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class FindTests : IDisposable
  {
    public FindTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void TestFindById()
    {
      var db = Database.Open();
      var user = db.Users.FindById(1);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestFindByIdWithCast()
    {
      var db = Database.Open();
      var user = (User)db.Users.FindById(1);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestFindByReturnsOne()
    {
      var db = Database.Open();
      var user = (User)db.Users.FindByName("Bob");
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestFindAllByName()
    {
      var db = Database.Open();
      IEnumerable<User> users = db.Users.FindAllByName("Bob").Cast<User>();
      Assert.Equal(1, users.Count());
    }

    [Fact]
    public void TestFindAllByNameAsIEnumerableOfDynamic()
    {
      var db = Database.Open();
      IEnumerable<dynamic> users = db.Users.FindAllByName("Bob");
      Assert.Equal(1, users.Count());
    }

    [Fact]
    public void TestFindAllByPartialName()
    {
      var db = Database.Open();
      IEnumerable<User> users = db.Users.FindAll(db.Users.Name.Like("Bob")).ToList<User>();
      Assert.Equal(1, users.Count());
    }

    [Fact]
    public void TestAllCount()
    {
      var db = Database.Open();
      var count = db.Users.All().ToList().Count;
      Assert.Equal(3, count);
    }

    [Fact]
    public void TestAllWithSkipCount()
    {
      var db = Database.Open();
      var count = db.Users.All().Skip(1).ToList().Count;
      Assert.Equal(2, count);
    }

    [Fact]
    public void TestImplicitCast()
    {
      var db = Database.Open();
      User user = db.Users.FindById(1);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void TestImplicitEnumerableCast()
    {
      var db = Database.Open();
      foreach (User user in db.Users.All())
      {
        Assert.NotNull(user);
      }
    }

    [Fact]
    public void TestFindWithSchemaQualification()
    {
      var db = Database.Open();
            
      var publicActual = db.Public.SchemaTable.FindById(1);
      var testActual = db.Test.SchemaTable.FindById(1);

      Assert.NotNull(publicActual);
      Assert.Equal("Pass", publicActual.Description);
      Assert.NotNull(testActual);
      Assert.Equal("Pass", testActual.Description);
    }

    [Fact]
    public void TestFindWithCriteriaAndSchemaQualification()
    {
      var db = Database.Open();

      var publicActual = db.Public.SchemaTable.Find(db.Public.SchemaTable.Id == 1);

      Assert.NotNull(publicActual);
      Assert.Equal("Pass", publicActual.Description);
    }

    [Fact]
    public void TestFindOnAView()
    {
      var db = Database.Open();
      var u = db.ViewCustomers.FindById(1);
      Assert.NotNull(u);
    }

    [Fact]
    public void TestCast()
    {
      var db = Database.Open();
      var userQuery = db.Users.All().Cast<User>() as IEnumerable<User>;
      Assert.NotNull(userQuery);
      var users = userQuery.ToList();
      Assert.NotEqual(0, users.Count);
    }
  }
}