using System;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class InsertTest
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
    public void TestInsertWithNamedArguments()
    {
      var db = Database.Open();

      var user = db.Public.Users.Insert(Name: "Ford", Password: "hoopy", Age: 29);

      Assert.IsNotNull(user);
      Assert.AreNotEqual(0, user.Id);
      Assert.AreEqual("Ford", user.Name);
      Assert.AreEqual("hoopy", user.Password);
      Assert.AreEqual(29, user.Age);
    }

    [Test]
    public void TestInsertWithStaticTypeObject()
    {
      var db = Database.Open();

      var user = new User {Name = "Zaphod", Password = "zarquon", Age = 42};

      var actual = db.Users.Insert(user);

      Assert.IsNotNull(user);
      Assert.AreNotEqual(0, actual.Id);
      Assert.AreEqual("Zaphod", actual.Name);
      Assert.AreEqual("zarquon", actual.Password);
      Assert.AreEqual(42, actual.Age);
    }

    [Test]
    public void TestMultiInsertWithStaticTypeObjects()
    {
      var db = Database.Open();

      var users = new[]
                    {
                      new User {Name = "Slartibartfast", Password = "bistromathics", Age = 777},
                      new User {Name = "Wowbagger", Password = "teatime", Age = int.MaxValue}
                    };

      IList<User> actuals = db.Users.Insert(users).ToList<User>();

      Assert.AreEqual(2, actuals.Count);
      Assert.AreNotEqual(0, actuals[0].Id);
      Assert.AreEqual("Slartibartfast", actuals[0].Name);
      Assert.AreEqual("bistromathics", actuals[0].Password);
      Assert.AreEqual(777, actuals[0].Age);

      Assert.AreNotEqual(0, actuals[1].Id);
      Assert.AreEqual("Wowbagger", actuals[1].Name);
      Assert.AreEqual("teatime", actuals[1].Password);
      Assert.AreEqual(int.MaxValue, actuals[1].Age);
    }

    [Test]
    public void TestInsertWithDynamicTypeObject()
    {
      var db = Database.Open();

      dynamic user = new ExpandoObject();
      user.Name = "Marvin";
      user.Password = "diodes";
      user.Age = 42000000;

      var actual = db.Users.Insert(user);

      Assert.IsNotNull(user);
      Assert.AreEqual("Marvin", actual.Name);
      Assert.AreEqual("diodes", actual.Password);
      Assert.AreEqual(42000000, actual.Age);
    }

    [Test]
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

      var users = new[] {user1, user2};

      IList<dynamic> actuals = db.Users.Insert(users).ToList();

      Assert.AreEqual(2, actuals.Count);
      Assert.AreNotEqual(0, actuals[0].Id);
      Assert.AreEqual("Slartibartfast", actuals[0].Name);
      Assert.AreEqual("bistromathics", actuals[0].Password);
      Assert.AreEqual(777, actuals[0].Age);

      Assert.AreNotEqual(0, actuals[1].Id);
      Assert.AreEqual("Wowbagger", actuals[1].Name);
      Assert.AreEqual("teatime", actuals[1].Password);
      Assert.AreEqual(int.MaxValue, actuals[1].Age);
    }

    [Test]
    public void InsertBasicTypes()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          SmallintField: Int16.MaxValue,
          IntegerField: Int32.MaxValue,
          BigintField: Int64.MaxValue,
          DecimalUnlimitedField: Decimal.MaxValue,
          Decimal102Field: 99999999.99,
          NumericUnlimitedField: Decimal.MaxValue - 1,
          Numeric102Field: 88888888.88,
          RealField: 1.0e37f,
          DoublePrecisionField: 1.0e308,
          MoneyField: "$99.98",
          CharacterVaryingUnlimitedField: "CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField",
          CharacterVarying30Field: "CharacterVarying30Field Charac",
          VarcharUnlimitedField: "VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField",
          Varchar30Field: "Varchar30Field Varchar30Field ",
          CharacterField: "q",
          Character10Field: "qrstuvwxyz",
          CharField: "l",
          Char10Field: "lmnopqrstu",
          TextField: "TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField",
          ByteaField: new byte[] {0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xb1, 0xb2, 0xb3},
          TimestampField: "2011/10/14 20:45:31",
          TimestampWithoutTimeZoneField: "2011/10/14 20:45:31",
          TimestamptzField: "2011/10/14 16:45:31 EDT",
          TimestampWithTimeZoneField: "2011/10/14 16:45:31 EDT",
          DateField: "2011/10/14",
          TimeField: "20:45:31",
          TimeWithoutTimeZoneField: "20:45:31",
          TimetzField: "16:45:31 EDT",
          TimeWithTimeZoneField: "16:45:31 EDT",
          IntervalField: "P1Y2MT5H"
        );

      Assert.NotNull(result);
      Assert.True(result.Id > 0);

      Assert.IsAssignableFrom<Int16>(result.SmallintField);
      Assert.AreEqual(Int16.MaxValue, result.SmallintField);

      Assert.IsAssignableFrom<Int32>(result.IntegerField);
      Assert.AreEqual(Int32.MaxValue, result.IntegerField);

      Assert.IsAssignableFrom<Int64>(result.BigintField);
      Assert.AreEqual(Int64.MaxValue, result.BigintField);

      Assert.IsAssignableFrom<Decimal>(result.DecimalUnlimitedField);
      Assert.AreEqual(Decimal.MaxValue, result.DecimalUnlimitedField);

      Assert.IsAssignableFrom<Decimal>(result.Decimal102Field);
      Assert.AreEqual(99999999.99, result.Decimal102Field);

      Assert.IsAssignableFrom<Decimal>(result.NumericUnlimitedField);
      Assert.AreEqual(Decimal.MaxValue - 1, result.NumericUnlimitedField);

      Assert.IsAssignableFrom<Decimal>(result.Numeric102Field);
      Assert.AreEqual(88888888.88, result.Numeric102Field);

      Assert.IsAssignableFrom<Single>(result.RealField);
      Assert.AreEqual(1e37f, result.RealField);

      Assert.IsAssignableFrom<Double>(result.DoublePrecisionField);
      Assert.AreEqual(1.0e308, result.DoublePrecisionField);

      Assert.IsAssignableFrom<Int32>(result.SerialField);
      Assert.True(result.SerialField > 0);

      Assert.IsAssignableFrom<Int64>(result.BigserialField);
      Assert.True(result.BigserialField > 0);

      Assert.IsAssignableFrom<Decimal>(result.MoneyField);
      Assert.AreEqual(99.98, result.MoneyField);
      
      Assert.IsAssignableFrom<String>(result.CharacterVaryingUnlimitedField);
      Assert.AreEqual("CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField", result.CharacterVaryingUnlimitedField);
      
      Assert.IsAssignableFrom<String>(result.CharacterVarying30Field);
      Assert.AreEqual("CharacterVarying30Field Charac", result.CharacterVarying30Field);
      
      Assert.IsAssignableFrom<String>(result.VarcharUnlimitedField);
      Assert.AreEqual("VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField", result.VarcharUnlimitedField);
      
      Assert.IsAssignableFrom<String>(result.Varchar30Field);
      Assert.AreEqual("Varchar30Field Varchar30Field ", result.Varchar30Field);
      
      Assert.IsAssignableFrom<String>(result.CharacterField);
      Assert.AreEqual("q", result.CharacterField);
      
      Assert.IsAssignableFrom<String>(result.Character10Field);
      Assert.AreEqual("qrstuvwxyz", result.Character10Field);
      
      Assert.IsAssignableFrom<String>(result.CharField);
      Assert.AreEqual("l", result.CharField);
      
      Assert.IsAssignableFrom<String>(result.Char10Field);
      Assert.AreEqual("lmnopqrstu", result.Char10Field);

      Assert.IsAssignableFrom<String>(result.TextField);
      Assert.AreEqual("TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField", result.TextField);
      
      Assert.IsAssignableFrom<Byte[]>(result.ByteaField);
      Assert.AreEqual(new byte[] { 0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xb1, 0xb2, 0xb3 }, result.ByteaField);

      Assert.IsAssignableFrom<DateTime>(result.TimestampField);
      var convertedDt = TimeZoneInfo.ConvertTimeFromUtc(result.TimestampField, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
      Assert.AreEqual(2011, convertedDt.Year);
      Assert.AreEqual(10, convertedDt.Month);
      Assert.AreEqual(14, convertedDt.Day);
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestampWithoutTimeZoneField);
      convertedDt = TimeZoneInfo.ConvertTimeFromUtc(result.TimestampWithoutTimeZoneField, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
      Assert.AreEqual(2011, convertedDt.Year);
      Assert.AreEqual(10, convertedDt.Month);
      Assert.AreEqual(14, convertedDt.Day);
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestamptzField);
      Assert.AreEqual(2011, result.TimestamptzField.Year);
      Assert.AreEqual(10, result.TimestamptzField.Month);
      Assert.AreEqual(14, result.TimestamptzField.Day);
      Assert.AreEqual(16, result.TimestamptzField.Hour);
      Assert.AreEqual(45, result.TimestamptzField.Minute);
      Assert.AreEqual(31, result.TimestamptzField.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestampWithTimeZoneField);
      Assert.AreEqual(2011, result.TimestampWithTimeZoneField.Year);
      Assert.AreEqual(10, result.TimestampWithTimeZoneField.Month);
      Assert.AreEqual(14, result.TimestampWithTimeZoneField.Day);
      Assert.AreEqual(16, result.TimestampWithTimeZoneField.Hour);
      Assert.AreEqual(45, result.TimestampWithTimeZoneField.Minute);
      Assert.AreEqual(31, result.TimestampWithTimeZoneField.Second);

      Assert.IsAssignableFrom<DateTime>(result.DateField);
      Assert.AreEqual(2011, result.DateField.Year);
      Assert.AreEqual(10, result.DateField.Month);
      Assert.AreEqual(14, result.DateField.Day);

      Assert.IsAssignableFrom<DateTime>(result.TimeField);
      convertedDt = TimeZoneInfo.ConvertTimeFromUtc(result.TimeField, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithoutTimeZoneField);
      convertedDt = TimeZoneInfo.ConvertTimeFromUtc(result.TimeWithoutTimeZoneField, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimetzField);
      Assert.AreEqual(16, result.TimetzField.Hour);
      Assert.AreEqual(45, result.TimetzField.Minute);
      Assert.AreEqual(31, result.TimetzField.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithTimeZoneField);
      Assert.AreEqual(16, result.TimeWithTimeZoneField.Hour);
      Assert.AreEqual(45, result.TimeWithTimeZoneField.Minute);
      Assert.AreEqual(31, result.TimeWithTimeZoneField.Second);

      Assert.IsAssignableFrom<TimeSpan>(result.IntervalField);
      Assert.AreEqual(1, result.IntervalField.Years);
      Assert.AreEqual(2, result.IntervalField.Months);
      Assert.AreEqual(0, result.IntervalField.Days);
      Assert.AreEqual(5, result.IntervalField.Hours);
      Assert.AreEqual(0, result.IntervalField.Minutes);
      Assert.AreEqual(0, result.IntervalField.Seconds);

    }
   
  }

  
}