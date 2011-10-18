using System;
using System.Dynamic;
using NUnit.Framework;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class ConversionTest
  {
    [SetUp]
    public void SetUp()
    {
      DatabaseUtility.SeedDatabase("Test");
    }

    [TearDown]
    public void TearDown()
    {
    }


    [Test]
    public void WeirdTypeGetsConvertedToInt()
    {
      var db = Database.Open();
      var user = db.Users.FindByName("Bob");

      var weirdValue = new WeirdType(user.Id);
      var result = db.Users.FindById(weirdValue);
      Assert.AreEqual(user.Id, result.Id);
    }

    [Test]
    public void WeirdTypeUsedInQueryGetsConvertedToInt()
    {
      var db = Database.Open();
      var user = db.Users.FindByName("Bob");

      var weirdValue = new WeirdType(user.Id);
      var result = db.Users.QueryById(weirdValue).FirstOrDefault();
      Assert.IsNotNull(result);
      Assert.AreEqual(user.Id, result.Id);
    }

    [Test]
    public void InsertingWeirdTypesFromExpando()
    {
      dynamic expando = new ExpandoObject();
      expando.Name = new WeirdType("Oddball");
      expando.Password = new WeirdType("Fish");
      expando.Age = new WeirdType(3);
      expando.ThisIsNotAColumn = new WeirdType("Submit");

      var db = Database.Open();
      var user = db.Users.Insert(expando);
      Assert.True(user.id is int);
      Assert.AreEqual("Oddball", user.Name);
      Assert.AreEqual("Fish", user.Password);
      Assert.AreEqual(3, user.Age);
    }
  }

  internal class WeirdType : DynamicObject
  {
    private readonly object _value;

    public WeirdType(object value)
    {
      _value = value;
    }

    public override bool TryConvert(ConvertBinder binder, out object result)
    {
      result = Convert.ChangeType(_value, binder.Type);
      return true;
    }

    public override string ToString()
    {
      return _value.ToString();
    }
  }
}