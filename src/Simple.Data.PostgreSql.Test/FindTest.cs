using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql.Test
{
  public class FindTest
  {
    [SetUp]
    public void SetUp()
    {
      GlobalTest.Database.Seed();
    }

    [Test]
    public void TestFindByName()
    {
      var db = Database.Open();
      var user = db.Public.Users.FindByName("Bob");
      Assert.NotNull(user);
      Assert.AreEqual("Bob", user.Name);
    }

    [Test]
    public void TestFindByNameWithCast()
    {
      var db = Database.Open();
      var user = (User) db.Public.Users.FindByName("Bob");
      Assert.NotNull(user);
      Assert.AreEqual("Bob", user.Name);
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

    [Test]
    public void TestAllWithSkipCount()
    {
      var db = Database.Open();
      var count = db.Users.All().Skip(1).ToList().Count;
      Assert.AreEqual(2, count);
    }

    [Test]
    public void TestAllWithSkipTake()
    {
      var db = Database.Open();
      List<User> users = db.Users.All().Skip(1).Take(2).ToList<User>();
      Assert.AreEqual(2, users.Count);
      Assert.AreEqual("Charlie", users[0].Name);
      Assert.AreEqual("Dave", users[1].Name);
    }

    [Test]
    public void TestAllWithSkipTakeTotalCount()
    {
      var db = Database.Open();
      Future<int> totalCount;
      var count = db.Users.All().WithTotalCount(out totalCount).Skip(1).Take(2).ToList().Count;
      Assert.AreEqual(3, totalCount);
      Assert.AreEqual(2, count);
    }

    [Test]
    public void TestAllWhereWithSkipTakeTotalCount()
    {
      var db = Database.Open();
      Future<int> totalCount;
      List<User> users = db.Users.All().Where(db.Users.Name.Like("%e")).WithTotalCount(out totalCount).Skip(1).ToList<User>();
      Assert.AreEqual(2, totalCount);
      Assert.AreEqual(1, users.Count);
      Assert.AreEqual("Dave", users[0].Name);
    }

    [Test]
    public void TestImplicitCast()
    {
      var db = Database.Open();
      User user = db.Users.FindByName("Bob");
      Assert.NotNull(user);
    }

    [Test]
    public void TestImplicitEnumerableCast()
    {
      var db = Database.Open();
      foreach (User user in db.Users.All())
      {
        Assert.NotNull(user);
      }
    }

    [Test]
    public void TestFindWithSchemaQualification()
    {
      var db = Database.Open();

      var publicActual = db.Public.SchemaTable.All().FirstOrDefault();
      var testActual = db.Test.SchemaTable.All().FirstOrDefault();

      Assert.NotNull(publicActual);
      Assert.AreEqual("Pass", publicActual.Description);
      Assert.NotNull(testActual);
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
      var customer = db.ViewCustomers.FindByName("Test");
      Assert.IsNotNull(customer);
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

    [Test]
    public void FindAllBasicTypes()
    {
      var db = Database.Open();
      var result = db.Public.BasicTypes.All().FirstOrDefault();

      Assert.NotNull(result);

      Assert.IsAssignableFrom<Int32>(result.Id);
      Assert.IsAssignableFrom<Int16>(result.SmallintField);
      Assert.IsAssignableFrom<Int32>(result.IntegerField);
      Assert.IsAssignableFrom<Int64>(result.BigintField);
      Assert.IsAssignableFrom<Decimal>(result.DecimalUnlimitedField);
      Assert.IsAssignableFrom<Decimal>(result.Decimal102Field);
      Assert.IsAssignableFrom<Decimal>(result.NumericUnlimitedField);
      Assert.IsAssignableFrom<Decimal>(result.Numeric102Field);
      Assert.IsAssignableFrom<Single>(result.RealField);
      Assert.IsAssignableFrom<Double>(result.DoublePrecisionField);
      Assert.IsAssignableFrom<Int32>(result.SerialField);
      Assert.IsAssignableFrom<Int64>(result.BigserialField);
      Assert.IsAssignableFrom<Decimal>(result.MoneyField);
      Assert.IsAssignableFrom<String>(result.CharacterVaryingUnlimitedField);
      Assert.IsAssignableFrom<String>(result.CharacterVarying30Field);
      Assert.IsAssignableFrom<String>(result.VarcharUnlimitedField);
      Assert.IsAssignableFrom<String>(result.Varchar30Field);
      Assert.IsAssignableFrom<String>(result.CharacterField);
      Assert.IsAssignableFrom<String>(result.Character10Field);
      Assert.IsAssignableFrom<String>(result.CharField);
      Assert.IsAssignableFrom<String>(result.Char10Field);
      Assert.IsAssignableFrom<String>(result.TextField);
      Assert.IsAssignableFrom<Byte[]>(result.ByteaField);
      Assert.IsAssignableFrom<DateTime>(result.TimestampField);
      Assert.IsAssignableFrom<DateTime>(result.TimestampWithoutTimeZoneField);
      Assert.IsAssignableFrom<DateTime>(result.TimestamptzField);
      Assert.IsAssignableFrom<DateTime>(result.TimestampWithTimeZoneField);
      Assert.IsAssignableFrom<DateTime>(result.DateField);
      Assert.IsAssignableFrom<DateTime>(result.TimeField);
      Assert.IsAssignableFrom<DateTime>(result.TimeWithoutTimeZoneField);
      Assert.IsAssignableFrom<DateTime>(result.TimetzField);
      Assert.IsAssignableFrom<DateTime>(result.TimeWithTimeZoneField);
      Assert.IsAssignableFrom<TimeSpan>(result.IntervalField);
      Assert.IsAssignableFrom<Boolean>(result.BooleanField);
      Assert.IsAssignableFrom<NpgsqlPoint>(result.PointField);
      Assert.IsAssignableFrom<NpgsqlLSeg>(result.LsegField);
      Assert.IsAssignableFrom<NpgsqlBox>(result.BoxField);
      Assert.IsAssignableFrom<NpgsqlPath>(result.PathClosedField);
      Assert.IsAssignableFrom<NpgsqlPath>(result.PathOpenField);
      Assert.IsAssignableFrom<NpgsqlPolygon>(result.PolygonField);
      Assert.IsAssignableFrom<NpgsqlCircle>(result.CircleField);
      Assert.IsAssignableFrom<String>(result.CidrField);
      Assert.IsAssignableFrom<System.Net.IPAddress>(result.InetField);
      Assert.IsAssignableFrom<String>(result.MacaddrField);
      Assert.IsAssignableFrom<Boolean>(result.BitField); // bit(1) is a special case.  Actual CLR type is BitString, but converts in the Npgsql driver to Boolean.
      Assert.IsAssignableFrom<BitString>(result.Bit10Field);
      Assert.IsAssignableFrom<String>(result.BitVaryingUnlimitedField);
      Assert.IsAssignableFrom<String>(result.BitVarying10Field);
      Assert.IsAssignableFrom<String>(result.TsvectorField);
      Assert.IsAssignableFrom<String>(result.TsqueryField);
      Assert.IsAssignableFrom<Guid>(result.UuidField);
      Assert.IsAssignableFrom<Int64>(result.OidField);
    }

    [Test]
    public void FindAllArrayTypes()
    {
      var db = Database.Open();
      var result = db.Public.ArrayTypes.All().FirstOrDefault();

      Assert.NotNull(result);

      Assert.IsAssignableFrom<Int32[]>(result.IntegerArrayField);
      Assert.IsAssignableFrom<Single[]>(result.RealArrayField);
      Assert.IsAssignableFrom<Double[]>(result.DoublePrecisionArrayField);
      //Assert.IsAssignableFrom<String[]>(result.VarcharArrayField); // TODO: Simple.data converts to a SimpleList.  Why?
      Assert.IsAssignableFrom<Int32[,]>(result.IntegerMultiArrayField);
      Assert.IsAssignableFrom<Single[,]>(result.RealMultiArrayField);
      Assert.IsAssignableFrom<Double[,]>(result.DoublePrecisionMultiArrayField);
      Assert.IsAssignableFrom<String[,]>(result.VarcharMultiArrayField);
    }
  }
}