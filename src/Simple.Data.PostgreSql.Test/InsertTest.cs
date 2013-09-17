using System;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql.Test
{
  public class InsertTest
  {
    [SetUp]
    public void SetUp()
    {
      GlobalTest.Database.Seed();
    }

    #region MA - added support for inserting enums

    internal class UserWithEnum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public AgeDescription Age { get; set; }
    }

    public enum AgeDescription
    {
        Young = 1,
        Old = 80
    }

    [Test]
    public void TestInsertWithStaticTypeObjectWithEnum()
    {
        var db = Database.Open();

        var user = new UserWithEnum
        {
            Name = "Zaphod",
            Password = "zarquon",
            Age = AgeDescription.Old
        };

        UserWithEnum actual = db.Users.Insert(user);

        Assert.IsNotNull(user);
        Assert.AreNotEqual(0, actual.Id);
        Assert.AreEqual("Zaphod", actual.Name);
        Assert.AreEqual("zarquon", actual.Password);
        Assert.AreEqual(80, (int)actual.Age);
        Assert.AreEqual(AgeDescription.Old, actual.Age);
    }

    [Test]
    public void TestInsertManyWithStaticTypeObjectWithEnum()
    {
        var db = Database.Open();

        var user1 = new UserWithEnum
        {
            Name = "Zaphod",
            Password = "zarquon",
            Age = AgeDescription.Old
        };
        var user2 = new UserWithEnum
        {
            Name = "Zaphod2",
            Password = "zarquon2",
            Age = AgeDescription.Young
        };

        List<UserWithEnum> actual = db.Users.Insert(new[]{user1, user2}).ToList<UserWithEnum>();

        Assert.IsNotNull(actual[0]);
        Assert.AreNotEqual(0, actual[0].Id);
        Assert.AreEqual("Zaphod", actual[0].Name);
        Assert.AreEqual("zarquon", actual[0].Password);
        Assert.AreEqual(80, (int)actual[0].Age);
        Assert.AreEqual(AgeDescription.Old, actual[0].Age);

        Assert.IsNotNull(actual[1]);
        Assert.AreNotEqual(0, actual[1].Id);
        Assert.AreEqual("Zaphod2", actual[1].Name);
        Assert.AreEqual("zarquon2", actual[1].Password);
        Assert.AreEqual(1, (int)actual[1].Age);
        Assert.AreEqual(AgeDescription.Young, actual[1].Age);
    }

    [Test]
    public void TestInsertWithObjectWithNullValue()
    {
        var db = Database.Open();

        var actual = db.Customers.Insert(Name: "customername", Address: null);

        Assert.AreNotEqual(0, actual.Id);
        Assert.AreEqual("customername", actual.Name);
        Assert.Null(actual.Address);
    }

    #endregion

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
          ByteaField: new byte[] {0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xb1, 0xb2, 0xb3},
          BooleanField: true,
          CidrField: "192.168.12",
          InetField: "127.0.0.1/32",
          MacaddrField: "01:02:03:04:05:06",
          TsvectorField: "cat fat flat mat rat splat",
          TsqueryField: "fat & rat & !cat",
          UuidField: "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
          OidField: 1
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

      Assert.IsAssignableFrom<Byte[]>(result.ByteaField);
      Assert.AreEqual(new byte[] {0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xb1, 0xb2, 0xb3}, result.ByteaField);

      Assert.IsAssignableFrom<Boolean>(result.BooleanField);
      Assert.AreEqual(true, result.BooleanField);

      Assert.IsAssignableFrom<String>(result.CidrField);
      Assert.AreEqual("192.168.12.0/24", result.CidrField);

      Assert.IsAssignableFrom<System.Net.IPAddress>(result.InetField);
      Assert.AreEqual(System.Net.IPAddress.Parse("127.0.0.1"), result.InetField);

      Assert.IsAssignableFrom<String>(result.MacaddrField);
      Assert.AreEqual("01:02:03:04:05:06", result.MacaddrField);

      Assert.IsAssignableFrom<String>(result.TsvectorField);
      Assert.AreEqual("'cat' 'fat' 'flat' 'mat' 'rat' 'splat'", result.TsvectorField);

      Assert.IsAssignableFrom<String>(result.TsqueryField);
      Assert.AreEqual("'fat' & 'rat' & !'cat'", result.TsqueryField);

      Assert.IsAssignableFrom<Guid>(result.UuidField);
      Assert.AreEqual(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), result.UuidField);

      Assert.IsAssignableFrom<Int64>(result.OidField);
      Assert.AreEqual(1, result.OidField);
    }

    [Test]
    public void InsertBasicTypesStringsAndBits()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          CharacterVaryingUnlimitedField: "CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField CharacterVaryingUnlimitedField",
          CharacterVarying30Field: "CharacterVarying30Field Charac",
          VarcharUnlimitedField: "VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField VarcharUnlimitedField",
          Varchar30Field: "Varchar30Field Varchar30Field ",
          CharacterField: "q",
          Character10Field: "qrstuvwxyz",
          CharField: "l",
          Char10Field: "lmnopqrstu",
          TextField: "TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField TextField",
          BitField: "1",
          Bit10Field: "1100110011",
          BitVaryingUnlimitedField: "1111001111001100110011001010101010",
          BitVarying10Field: "101"
          );

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

      Assert.IsAssignableFrom<Boolean>(result.BitField);
      Assert.AreEqual(true, result.BitField);

      Assert.IsAssignableFrom<BitString>(result.Bit10Field);
      Assert.AreEqual(new BitString("1100110011"), result.Bit10Field);

      Assert.IsAssignableFrom<String>(result.BitVaryingUnlimitedField);
      Assert.AreEqual("1111001111001100110011001010101010", result.BitVaryingUnlimitedField);

      Assert.IsAssignableFrom<String>(result.BitVarying10Field);
      Assert.AreEqual("101", result.BitVarying10Field);
    }


    [Test]
    public void InsertBasicTypesDatesAndTimesUsingStrings()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          TimestampField: "2011/10/14 20:45:31",
          TimestampWithoutTimeZoneField: "2011/10/14 20:45:31",
          TimestamptzField: "2011/10/14 16:45:31 " + TimeZoneInfo.Local.BaseUtcOffset.ToString(),
          TimestampWithTimeZoneField: "2011/10/14 16:45:31 " + TimeZoneInfo.Local.BaseUtcOffset.ToString(),
          DateField: "2011/10/14",
          TimeField: "20:45:31",
          TimeWithoutTimeZoneField: "20:45:31",
          TimetzField: "16:45:31 " + TimeZoneInfo.Local.BaseUtcOffset.ToString(),
          TimeWithTimeZoneField: "16:45:31 " + TimeZoneInfo.Local.BaseUtcOffset.ToString(),
          IntervalField: "P1Y2MT5H5M10S"
          );

      Assert.IsAssignableFrom<DateTime>(result.TimestampField);
      var convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(2011, convertedDt.Year);
      Assert.AreEqual(10, convertedDt.Month);
      Assert.AreEqual(14, convertedDt.Day);
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestampWithoutTimeZoneField);
      convertedDt = result.TimestampWithoutTimeZoneField.ToLocalTime();
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
      //Assert.AreEqual(16, result.TimestamptzField.Hour);  // TODO: Conversion into DateTime of a timestamp with time zone does not work correctly.
      Assert.AreEqual(45, result.TimestamptzField.Minute);
      Assert.AreEqual(31, result.TimestamptzField.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestampWithTimeZoneField);
      Assert.AreEqual(2011, result.TimestampWithTimeZoneField.Year);
      Assert.AreEqual(10, result.TimestampWithTimeZoneField.Month);
      Assert.AreEqual(14, result.TimestampWithTimeZoneField.Day);
      //Assert.AreEqual(16, result.TimestampWithTimeZoneField.Hour);  // TODO: Conversion into DateTime of a timestamp with time zone does not work correctly.
      Assert.AreEqual(45, result.TimestampWithTimeZoneField.Minute);
      Assert.AreEqual(31, result.TimestampWithTimeZoneField.Second);

      Assert.IsAssignableFrom<DateTime>(result.DateField);
      Assert.AreEqual(2011, result.DateField.Year);
      Assert.AreEqual(10, result.DateField.Month);
      Assert.AreEqual(14, result.DateField.Day);

      Assert.IsAssignableFrom<DateTime>(result.TimeField);
      convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithoutTimeZoneField);
      convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimetzField);
      //Assert.AreEqual(16, result.TimetzField.Hour);  // TODO: Conversion into DateTime of a time with time zone does not work correctly.
      Assert.AreEqual(45, result.TimetzField.Minute);
      Assert.AreEqual(31, result.TimetzField.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithTimeZoneField);
      //Assert.AreEqual(16, result.TimeWithTimeZoneField.Hour);  // TODO: Conversion into DateTime of a time with time zone does not work correctly.
      Assert.AreEqual(45, result.TimeWithTimeZoneField.Minute);
      Assert.AreEqual(31, result.TimeWithTimeZoneField.Second);

      Assert.IsAssignableFrom<TimeSpan>(result.IntervalField);
      Assert.AreEqual(420, result.IntervalField.Days);
      Assert.AreEqual(5, result.IntervalField.Hours);
      Assert.AreEqual(5, result.IntervalField.Minutes);
      Assert.AreEqual(10, result.IntervalField.Seconds);
    }


    [Test]
    public void InsertBasicTypesDatesAndTimesUsingObjects()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          TimestampField: new DateTime(2011, 10, 14, 20, 45, 31, DateTimeKind.Utc),
          TimestampWithoutTimeZoneField: new DateTime(2011, 10, 14, 20, 45, 31, DateTimeKind.Utc),
          TimestamptzField: new DateTime(2011, 10, 14, 16, 45, 31, DateTimeKind.Local),
          TimestampWithTimeZoneField: new DateTime(2011, 10, 14, 16, 45, 31, DateTimeKind.Local),
          DateField: new DateTime(2011, 10, 14),
          TimeField: new DateTime(2000, 1, 1, 20, 45, 31, DateTimeKind.Utc),
          TimeWithoutTimeZoneField: new DateTime(2000, 1, 1, 20, 45, 31, DateTimeKind.Utc),
          TimetzField: new DateTime(2000, 1, 1, 16, 45, 31, DateTimeKind.Local),
          TimeWithTimeZoneField: new DateTime(2000, 1, 1, 16, 45, 31, DateTimeKind.Local),
          IntervalField: new TimeSpan(420, 5, 5, 10)
          );

      Assert.IsAssignableFrom<DateTime>(result.TimestampField);
      var convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(2011, convertedDt.Year);
      Assert.AreEqual(10, convertedDt.Month);
      Assert.AreEqual(14, convertedDt.Day);
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimestampWithoutTimeZoneField);
      convertedDt = result.TimestampWithoutTimeZoneField.ToLocalTime();
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
      convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithoutTimeZoneField);
      convertedDt = result.TimestampField.ToLocalTime();
      Assert.AreEqual(16, convertedDt.Hour);
      Assert.AreEqual(45, convertedDt.Minute);
      Assert.AreEqual(31, convertedDt.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimetzField);
      //Assert.AreEqual(16, result.TimetzField.Hour);  // TODO: Conversion into DateTime of a time with time zone does not work correctly.
      Assert.AreEqual(45, result.TimetzField.Minute);
      Assert.AreEqual(31, result.TimetzField.Second);

      Assert.IsAssignableFrom<DateTime>(result.TimeWithTimeZoneField);
      //Assert.AreEqual(16, result.TimeWithTimeZoneField.Hour);  // TODO: Conversion into DateTime of a time with time zone does not work correctly.
      Assert.AreEqual(45, result.TimeWithTimeZoneField.Minute);
      Assert.AreEqual(31, result.TimeWithTimeZoneField.Second);

      Assert.IsAssignableFrom<TimeSpan>(result.IntervalField);
      Assert.AreEqual(420, result.IntervalField.Days);
      Assert.AreEqual(5, result.IntervalField.Hours);
      Assert.AreEqual(5, result.IntervalField.Minutes);
      Assert.AreEqual(10, result.IntervalField.Seconds);
    }

    [Test]
    public void InsertBasicTypesGeometryUsingStrings()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          PointField: "(1,2)",
          LsegField: "((1,2),(3,4))",
          BoxField: "(1,2),(3,4)",
          PathClosedField: "((1,2),(3,4),(5,6))",
          PathOpenField: "[(1,2),(3,4),(5,6)]",
          PolygonField: "((1,2),(3,4),(5,6))",
          CircleField: "<(1,2),3>"
          );

      Assert.IsAssignableFrom<NpgsqlPoint>(result.PointField);
      Assert.AreEqual(1, result.PointField.X);
      Assert.AreEqual(2, result.PointField.Y);

      Assert.IsAssignableFrom<NpgsqlLSeg>(result.LsegField);
      Assert.AreEqual(1, result.LsegField.Start.X);
      Assert.AreEqual(2, result.LsegField.Start.Y);
      Assert.AreEqual(3, result.LsegField.End.X);
      Assert.AreEqual(4, result.LsegField.End.Y);

      Assert.IsAssignableFrom<NpgsqlBox>(result.BoxField);
      Assert.AreEqual(1, result.BoxField.Left);
      Assert.AreEqual(2, result.BoxField.Bottom);
      Assert.AreEqual(3, result.BoxField.Right);
      Assert.AreEqual(4, result.BoxField.Top);

      Assert.IsAssignableFrom<NpgsqlPath>(result.PathClosedField);
      Assert.AreEqual(3, result.PathClosedField.Count);
      Assert.AreEqual(false, result.PathClosedField.Open);
      Assert.AreEqual(1, result.PathClosedField[0].X);
      Assert.AreEqual(2, result.PathClosedField[0].Y);
      Assert.AreEqual(3, result.PathClosedField[1].X);
      Assert.AreEqual(4, result.PathClosedField[1].Y);
      Assert.AreEqual(5, result.PathClosedField[2].X);
      Assert.AreEqual(6, result.PathClosedField[2].Y);

      Assert.IsAssignableFrom<NpgsqlPath>(result.PathOpenField);
      Assert.AreEqual(3, result.PathOpenField.Count);
      Assert.AreEqual(true, result.PathOpenField.Open);
      Assert.AreEqual(1, result.PathOpenField[0].X);
      Assert.AreEqual(2, result.PathOpenField[0].Y);
      Assert.AreEqual(3, result.PathOpenField[1].X);
      Assert.AreEqual(4, result.PathOpenField[1].Y);
      Assert.AreEqual(5, result.PathOpenField[2].X);
      Assert.AreEqual(6, result.PathOpenField[2].Y);

      Assert.IsAssignableFrom<NpgsqlPolygon>(result.PolygonField);
      Assert.AreEqual(3, result.PolygonField.Count);
      Assert.AreEqual(1, result.PolygonField[0].X);
      Assert.AreEqual(2, result.PolygonField[0].Y);
      Assert.AreEqual(3, result.PolygonField[1].X);
      Assert.AreEqual(4, result.PolygonField[1].Y);
      Assert.AreEqual(5, result.PolygonField[2].X);
      Assert.AreEqual(6, result.PolygonField[2].Y);

      Assert.IsAssignableFrom<NpgsqlCircle>(result.CircleField);
      Assert.AreEqual(1, result.CircleField.Center.X);
      Assert.AreEqual(2, result.CircleField.Center.Y);
      Assert.AreEqual(3, result.CircleField.Radius);
    }

    [Test]
    public void InsertBasicTypesGeometryUsingObjects()
    {
      var db = Database.Open();

      var result =
        db.BasicTypes.Insert(
          PointField: new NpgsqlPoint(1, 2),
          LsegField: new NpgsqlLSeg(new NpgsqlPoint(1, 2), new NpgsqlPoint(3, 4)),
          BoxField: new NpgsqlBox(4, 3, 2, 1),
          PathClosedField: new NpgsqlPath(new[] {new NpgsqlPoint(1, 2), new NpgsqlPoint(3, 4), new NpgsqlPoint(5, 6)}, false),
          PathOpenField: new NpgsqlPath(new[] {new NpgsqlPoint(1, 2), new NpgsqlPoint(3, 4), new NpgsqlPoint(5, 6)}, true),
          PolygonField: new NpgsqlPolygon(new[] {new NpgsqlPoint(1, 2), new NpgsqlPoint(3, 4), new NpgsqlPoint(5, 6)}),
          CircleField: new NpgsqlCircle(new NpgsqlPoint(1, 2), 3)
          );

      Assert.IsAssignableFrom<NpgsqlPoint>(result.PointField);
      Assert.AreEqual(1, result.PointField.X);
      Assert.AreEqual(2, result.PointField.Y);

      Assert.IsAssignableFrom<NpgsqlLSeg>(result.LsegField);
      Assert.AreEqual(1, result.LsegField.Start.X);
      Assert.AreEqual(2, result.LsegField.Start.Y);
      Assert.AreEqual(3, result.LsegField.End.X);
      Assert.AreEqual(4, result.LsegField.End.Y);

      Assert.IsAssignableFrom<NpgsqlBox>(result.BoxField);
      Assert.AreEqual(1, result.BoxField.Left);
      Assert.AreEqual(2, result.BoxField.Bottom);
      Assert.AreEqual(3, result.BoxField.Right);
      Assert.AreEqual(4, result.BoxField.Top);

      Assert.IsAssignableFrom<NpgsqlPath>(result.PathClosedField);
      Assert.AreEqual(3, result.PathClosedField.Count);
      //Assert.AreEqual(false, result.PathClosedField.Open); // TODO: There appears to be a bug in Npgsql that saves all paths as open paths
      Assert.AreEqual(1, result.PathClosedField[0].X);
      Assert.AreEqual(2, result.PathClosedField[0].Y);
      Assert.AreEqual(3, result.PathClosedField[1].X);
      Assert.AreEqual(4, result.PathClosedField[1].Y);
      Assert.AreEqual(5, result.PathClosedField[2].X);
      Assert.AreEqual(6, result.PathClosedField[2].Y);

      Assert.IsAssignableFrom<NpgsqlPath>(result.PathOpenField);
      Assert.AreEqual(3, result.PathOpenField.Count);
      Assert.AreEqual(true, result.PathOpenField.Open);
      Assert.AreEqual(1, result.PathOpenField[0].X);
      Assert.AreEqual(2, result.PathOpenField[0].Y);
      Assert.AreEqual(3, result.PathOpenField[1].X);
      Assert.AreEqual(4, result.PathOpenField[1].Y);
      Assert.AreEqual(5, result.PathOpenField[2].X);
      Assert.AreEqual(6, result.PathOpenField[2].Y);

      Assert.IsAssignableFrom<NpgsqlPolygon>(result.PolygonField);
      Assert.AreEqual(3, result.PolygonField.Count);
      Assert.AreEqual(1, result.PolygonField[0].X);
      Assert.AreEqual(2, result.PolygonField[0].Y);
      Assert.AreEqual(3, result.PolygonField[1].X);
      Assert.AreEqual(4, result.PolygonField[1].Y);
      Assert.AreEqual(5, result.PolygonField[2].X);
      Assert.AreEqual(6, result.PolygonField[2].Y);

      Assert.IsAssignableFrom<NpgsqlCircle>(result.CircleField);
      Assert.AreEqual(1, result.CircleField.Center.X);
      Assert.AreEqual(2, result.CircleField.Center.Y);
      Assert.AreEqual(3, result.CircleField.Radius);
    }

    [Test]
    public void InsertArrayTypesWithStrings()
    {
      var db = Database.Open();

      var result =
        db.ArrayTypes.Insert(
          IntegerArrayField: "{1,2,3,4,5,6}",
          RealArrayField: "{1.1,2.2,3.3,4.4,5.5,6.6}",
          DoublePrecisionArrayField: "{1.1,2.2,3.3,4.4,5.5,6.6}",
          VarcharArrayField: "{one,two,three,four,five,six}",
          IntegerMultiArrayField: "{{1,2,3,4,5,6},{1,2,3,4,5,6}}",
          RealMultiArrayField: "{{1.1,2.2,3.3,4.4,5.5,6.6},{1.1,2.2,3.3,4.4,5.5,6.6}}",
          DoublePrecisionMultiArrayField: "{{1.1,2.2,3.3,4.4,5.5,6.6},{1.1,2.2,3.3,4.4,5.5,6.6}}",
          VarcharMultiArrayField: "{{one,two,three,four,five,six},{one,two,three,four,five,six}}"
          );

      Assert.NotNull(result);
      Assert.True(result.Id > 0);

      Assert.IsAssignableFrom<Int32[]>(result.IntegerArrayField);
      Assert.AreEqual(new[] {1, 2, 3, 4, 5, 6}, result.IntegerArrayField);

      Assert.IsAssignableFrom<Single[]>(result.RealArrayField);
      Assert.AreEqual(new[] {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}, result.RealArrayField);

      Assert.IsAssignableFrom<Double[]>(result.DoublePrecisionArrayField);
      Assert.AreEqual(new[] {1.1, 2.2, 3.3, 4.4, 5.5, 6.6}, result.DoublePrecisionArrayField);

      // TODO: Simple.data converts to a SimpleList.  Why?
      //Assert.IsAssignableFrom<String[]>(result.VarcharArrayField);
      Assert.AreEqual(new[] {"one", "two", "three", "four", "five", "six"}, result.VarcharArrayField);

      Assert.IsAssignableFrom<Int32[,]>(result.IntegerMultiArrayField);
      Assert.AreEqual(new[,] {{1, 2, 3, 4, 5, 6}, {1, 2, 3, 4, 5, 6}}, result.IntegerMultiArrayField);

      Assert.IsAssignableFrom<Single[,]>(result.RealMultiArrayField);
      Assert.AreEqual(new[,] {{1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}, {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}}, result.RealMultiArrayField);

      Assert.IsAssignableFrom<Double[,]>(result.DoublePrecisionMultiArrayField);
      Assert.AreEqual(new[,] {{1.1, 2.2, 3.3, 4.4, 5.5, 6.6}, {1.1, 2.2, 3.3, 4.4, 5.5, 6.6}}, result.DoublePrecisionMultiArrayField);

      Assert.IsAssignableFrom<String[,]>(result.VarcharMultiArrayField);
      Assert.AreEqual(new[,] {{"one", "two", "three", "four", "five", "six"}, {"one", "two", "three", "four", "five", "six"}}, result.VarcharMultiArrayField);
    }


    [Test]
    public void InsertArrayTypesWithObjects()
    {
      var db = Database.Open();

      var result =
        db.ArrayTypes.Insert(
          IntegerArrayField: new[] {1, 2, 3, 4, 5, 6},
          RealArrayField: new[] {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f},
          DoublePrecisionArrayField: new[] {1.1, 2.2, 3.3, 4.4, 5.5, 6.6},
          VarcharArrayField: new[] {"one", "two", "three", "four", "five", "six"},
          IntegerMultiArrayField: new[,] {{1, 2, 3, 4, 5, 6}, {1, 2, 3, 4, 5, 6}},
          RealMultiArrayField: new[,] {{1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}, {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}},
          DoublePrecisionMultiArrayField: new[,] {{1.1, 2.2, 3.3, 4.4, 5.5, 6.6}, {1.1, 2.2, 3.3, 4.4, 5.5, 6.6}},
          VarcharMultiArrayField: new[,] {{"one", "two", "three", "four", "five", "six"}, {"one", "two", "three", "four", "five", "six"}}
          );

      Assert.NotNull(result);
      Assert.True(result.Id > 0);

      Assert.IsAssignableFrom<Int32[]>(result.IntegerArrayField);
      Assert.AreEqual(new[] {1, 2, 3, 4, 5, 6}, result.IntegerArrayField);

      Assert.IsAssignableFrom<Single[]>(result.RealArrayField);
      Assert.AreEqual(new[] {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}, result.RealArrayField);

      Assert.IsAssignableFrom<Double[]>(result.DoublePrecisionArrayField);
      Assert.AreEqual(new[] {1.1, 2.2, 3.3, 4.4, 5.5, 6.6}, result.DoublePrecisionArrayField);

      //Assert.IsAssignableFrom<String[]>(result.VarcharArrayField); // TODO: Simple.data converts to a SimpleList.  Why?
      Assert.AreEqual(new[] {"one", "two", "three", "four", "five", "six"}, result.VarcharArrayField);

      Assert.IsAssignableFrom<Int32[,]>(result.IntegerMultiArrayField);
      Assert.AreEqual(new[,] {{1, 2, 3, 4, 5, 6}, {1, 2, 3, 4, 5, 6}}, result.IntegerMultiArrayField);

      Assert.IsAssignableFrom<Single[,]>(result.RealMultiArrayField);
      Assert.AreEqual(new[,] {{1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}, {1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f}}, result.RealMultiArrayField);

      Assert.IsAssignableFrom<Double[,]>(result.DoublePrecisionMultiArrayField);
      Assert.AreEqual(new[,] {{1.1, 2.2, 3.3, 4.4, 5.5, 6.6}, {1.1, 2.2, 3.3, 4.4, 5.5, 6.6}}, result.DoublePrecisionMultiArrayField);

      Assert.IsAssignableFrom<String[,]>(result.VarcharMultiArrayField);
      Assert.AreEqual(new[,] {{"one", "two", "three", "four", "five", "six"}, {"one", "two", "three", "four", "five", "six"}}, result.VarcharMultiArrayField);
    }
  }
}