using System;
using System.Dynamic;
using NUnit.Framework;

namespace Simple.Data.PostgreSqlTest
{
  public class ConversionTests
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
    public void WeirdTypeGetsConvertedToInt()
    {
      var weirdValue = new WeirdType(1);
      var db = Database.Open();
      var user = db.Users.FindById(weirdValue);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void WeirdTypeUsedInQueryGetsConvertedToInt()
    {
      var weirdValue = new WeirdType(1);
      var db = Database.Open();
      var user = db.Users.QueryById(weirdValue).FirstOrDefault();
      Assert.IsNotNull(user);
      Assert.AreEqual(1, user.Id);
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

  class WeirdType : DynamicObject
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
