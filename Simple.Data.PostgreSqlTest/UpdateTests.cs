using System.Dynamic;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class UpdateTests
  {
    public UpdateTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void TestUpdateWithNamedArguments()
    {
      var db = Database.Open();

      db.Users.UpdateById(Id: 1, Name: "Ford", Password: "hoopy", Age: 29);
      var user = db.Users.FindById(1);
      Assert.NotNull(user);
      Assert.Equal("Ford", user.Name);
      Assert.Equal("hoopy", user.Password);
      Assert.Equal(29, user.Age);
    }

    [Fact]
    public void TestUpdateWithStaticTypeObject()
    {
      var db = Database.Open();

      var user = new User {Id = 2, Name = "Zaphod", Password = "zarquon", Age = 42};

      db.Users.Update(user);

      User actual = db.Users.FindById(2);

      Assert.NotNull(user);
      Assert.Equal("Zaphod", actual.Name);
      Assert.Equal("zarquon", actual.Password);
      Assert.Equal(42, actual.Age);
    }

    [Fact]
    public void TestUpdateWithDynamicTypeObject()
    {
      var db = Database.Open();

      dynamic user = new ExpandoObject();
      user.Id = 3;
      user.Name = "Marvin";
      user.Password = "diodes";
      user.Age = 42000000;

      db.Users.Update(user);

      var actual = db.Users.FindById(3);

      Assert.NotNull(user);
      Assert.Equal("Marvin", actual.Name);
      Assert.Equal("diodes", actual.Password);
      Assert.Equal(42000000, actual.Age);
    }

    /* TODO
    [Fact]
    public void TestUpdateWithVarBinaryMaxColumn()
    {
      var db = Database.Open();
      var blob = new Blob
                   {
                     Id = 1,
                     Data = new byte[] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
                   };
      db.Blobs.Insert(blob);

      var newData = blob.Data = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      db.Blobs.Update(blob);

      blob = db.Blobs.FindById(1);

      Assert.IsTrue(newData.SequenceEqual(blob.Data));
    }
     */
  }
}