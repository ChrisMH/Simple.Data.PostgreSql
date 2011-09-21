using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlFact
{
  public class ConversionTests : IDisposable
  {
    public ConversionTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }


    [Fact]
    public void WeirdTypeGetsConvertedToInt()
    {
      var weirdValue = new WeirdType(1);
      var db = Database.Open();
      var user = db.Users.FindById(weirdValue);
      Assert.Equal(1, user.Id);
    }

    [Fact]
    public void WeirdTypeUsedInQueryGetsConvertedToInt()
    {
      var weirdValue = new WeirdType(1);
      var db = Database.Open();
      var user = db.Users.QueryById(weirdValue).FirstOrDefault();
      Assert.NotNull(user);
      Assert.Equal(1, user.Id);
    }

    [Fact]
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
      Assert.Equal("Oddball", user.Name);
      Assert.Equal("Fish", user.Password);
      Assert.Equal(3, user.Age);
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
