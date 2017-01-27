using System.Dynamic;

using NUnit.Framework;

namespace Simple.Data.Npgsql.Test
{
  public class UpdateTest
  {
    [SetUp]
    public void SetUp()
    {
      GlobalTest.Database.Seed();
    }

    [Test]
    public void TestUpdateWithNamedArguments()
    {
      var db = Database.Open();

      var userId = db.Users.FindByName("Bob").Id;

      db.Users.UpdateById(Id: userId, Name: "Ford", Password: "hoopy", Age: 29);
      var user = db.Users.FindById(userId);
      Assert.IsNotNull(user);
      Assert.AreEqual("Ford", user.Name);
      Assert.AreEqual("hoopy", user.Password);
      Assert.AreEqual(29, user.Age);
    }

    [Test]
    public void TestUpdateWithStaticTypeObject()
    {
      var db = Database.Open();

      var userId = db.Public.Users.FindByName("Charlie").Id;

      var user = new User {Id = userId, Name = "Zaphod", Password = "zarquon", Age = 42};

      db.Users.Update(user);

      User actual = db.Users.FindById(userId);

      Assert.IsNotNull(user);
      Assert.AreEqual("Zaphod", actual.Name);
      Assert.AreEqual("zarquon", actual.Password);
      Assert.AreEqual(42, actual.Age);
    }

    [Test]
    public void TestUpdateWithDynamicTypeObject()
    {
      var db = Database.Open();

      var userId = db.Public.Users.FindByName("Dave").Id;

      dynamic user = new ExpandoObject();
      user.Id = userId;
      user.Name = "Marvin";
      user.Password = "diodes";
      user.Age = 42000000;

      db.Users.Update(user);

      var actual = db.Users.FindById(userId);

      Assert.IsNotNull(user);
      Assert.AreEqual("Marvin", actual.Name);
      Assert.AreEqual("diodes", actual.Password);
      Assert.AreEqual(42000000, actual.Age);
    }

    /* TODO
    [Test]
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