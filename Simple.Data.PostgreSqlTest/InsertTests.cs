using System;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace Simple.Data.PostgreSqlTest
{
  public class InsertTests : IDisposable
  {
    public InsertTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void TestInsertWithNamedArguments()
    {
      var db = Database.Open();

      var user = db.Public.Users.Insert(Name: "Ford", Password: "hoopy", Age: 29);

      Assert.NotNull(user);
      Assert.Equal("Ford", user.Name);
      Assert.Equal("hoopy", user.Password);
      Assert.Equal(29, user.Age);
    }

    [Fact]
    public void TestInsertWithStaticTypeObject()
    {
      var db = Database.Open();

      var user = new User { Name = "Zaphod", Password = "zarquon", Age = 42 };

      var actual = db.Users.Insert(user);

      Assert.NotNull(user);
      Assert.Equal("Zaphod", actual.Name);
      Assert.Equal("zarquon", actual.Password);
      Assert.Equal(42, actual.Age);
    }

    [Fact]
    public void TestMultiInsertWithStaticTypeObjects()
    {
      var db = Database.Open();

      var users = new[]
                            {
                                new User { Name = "Slartibartfast", Password = "bistromathics", Age = 777 },
                                new User { Name = "Wowbagger", Password = "teatime", Age = int.MaxValue }
                            };

      IList<User> actuals = db.Users.Insert(users).ToList<User>();

      Assert.Equal(2, actuals.Count);
      Assert.NotEqual(0, actuals[0].Id);
      Assert.Equal("Slartibartfast", actuals[0].Name);
      Assert.Equal("bistromathics", actuals[0].Password);
      Assert.Equal(777, actuals[0].Age);

      Assert.NotEqual(0, actuals[1].Id);
      Assert.Equal("Wowbagger", actuals[1].Name);
      Assert.Equal("teatime", actuals[1].Password);
      Assert.Equal(int.MaxValue, actuals[1].Age);
    }

    [Fact]
    public void TestInsertWithDynamicTypeObject()
    {
      var db = Database.Open();

      dynamic user = new ExpandoObject();
      user.Name = "Marvin";
      user.Password = "diodes";
      user.Age = 42000000;

      var actual = db.Users.Insert(user);

      Assert.NotNull(user);
      Assert.Equal("Marvin", actual.Name);
      Assert.Equal("diodes", actual.Password);
      Assert.Equal(42000000, actual.Age);
    }

    [Fact]
    public void TestMultiInsertWithDynamicTypeObjects()
    {
      var db = Database.Open();

      dynamic user1 = new ExpandoObject();
      user1.Name = "Slartibartfast";
      user1.Password = "bistromathics";
      user1.Age = 777;

      dynamic user2 = new ExpandoObject();
      user2.Name = "Wowbagger";
      user2.Password = "teatime";
      user2.Age = int.MaxValue;

      var users = new[] { user1, user2 };

      IList<dynamic> actuals = db.Users.Insert(users).ToList();

      Assert.Equal(2, actuals.Count);
      Assert.NotEqual(0, actuals[0].Id);
      Assert.Equal("Slartibartfast", actuals[0].Name);
      Assert.Equal("bistromathics", actuals[0].Password);
      Assert.Equal(777, actuals[0].Age);

      Assert.NotEqual(0, actuals[1].Id);
      Assert.Equal("Wowbagger", actuals[1].Name);
      Assert.Equal("teatime", actuals[1].Password);
      Assert.Equal(int.MaxValue, actuals[1].Age);
    }

    /* TODO: Implement test for ByteA
    [Fact]
    public void TestWithImageColumn()
    {
      var db = Database.Open();
      try
      {
        var image = GetImage.Image;
        db.Images.Insert(Id: 1, TheImage: image);
        var img = (DbImage)db.Images.FindById(1);
        Assert.IsTrue(image.SequenceEqual(img.TheImage));
      }
      finally
      {
        db.Images.DeleteById(1);
      }
    }

    [Fact]
    public void TestInsertWithVarBinaryMaxColumn()
    {
      var db = Database.Open();
      var image = GetImage.Image;
      var blob = new Blob
      {
        Id = 1,
        Data = image
      };
      db.Blobs.Insert(blob);
      blob = db.Blobs.FindById(1);
      Assert.IsTrue(image.SequenceEqual(blob.Data));
    }
     */
  }
}
