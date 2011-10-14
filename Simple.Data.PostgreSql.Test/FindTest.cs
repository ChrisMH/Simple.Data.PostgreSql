using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NpgsqlTypes;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class FindTest
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
    public void TestFindById()
    {
      var db = Database.Open();
      var user = db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestFindByIdWithCast()
    {
      var db = Database.Open();
      var user = (User) db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestFindByReturnsOne()
    {
      var db = Database.Open();
      var user = (User) db.Users.FindByName("Bob");
      Assert.AreEqual(1, user.Id);
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

    /*
    [Test]
    public void TestAllWithSkipCount()
    {
      var db = Database.Open();
      var count = db.Users.All().Skip(1).ToList().Count;
      Assert.AreEqual(2, count);
    }
    */

    [Test]
    public void TestImplicitCast()
    {
      var db = Database.Open();
      User user = db.Users.FindById(1);
      Assert.AreEqual(1, user.Id);
    }

    [Test]
    public void TestImplicitEnumerableCast()
    {
      var db = Database.Open();
      foreach (User user in db.Users.All())
      {
        Assert.IsNotNull(user);
      }
    }

    [Test]
    public void TestFindWithSchemaQualification()
    {
      var db = Database.Open();

      var publicActual = db.Public.SchemaTable.FindById(1);
      var testActual = db.Test.SchemaTable.FindById(1);

      Assert.IsNotNull(publicActual);
      Assert.AreEqual("Pass", publicActual.Description);
      Assert.IsNotNull(testActual);
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
      var u = db.ViewCustomers.FindById(1);
      Assert.IsNotNull(u);
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
      var result = db.BasicTypes.FindById(1) as IDictionary<string, object>;

      Assert.NotNull(result);

      Assert.True(result.ContainsKey("Id"));
      Assert.IsAssignableFrom<Int32>(result["Id"]);

      Assert.True(result.ContainsKey("SmallintField"));
      Assert.IsAssignableFrom<Int16>(result["SmallintField"]);

      Assert.True(result.ContainsKey("IntegerField"));
      Assert.IsAssignableFrom<Int32>(result["IntegerField"]);

      Assert.True(result.ContainsKey("BigintField"));
      Assert.IsAssignableFrom<Int64>(result["BigintField"]);

      Assert.True(result.ContainsKey("DecimalUnlimitedField"));
      Assert.IsAssignableFrom<Decimal>(result["DecimalUnlimitedField"]);

      Assert.True(result.ContainsKey("Decimal102Field"));
      Assert.IsAssignableFrom<Decimal>(result["Decimal102Field"]);

      Assert.True(result.ContainsKey("NumericUnlimitedField"));
      Assert.IsAssignableFrom<Decimal>(result["NumericUnlimitedField"]);

      Assert.True(result.ContainsKey("Numeric102Field"));
      Assert.IsAssignableFrom<Decimal>(result["Numeric102Field"]);

      Assert.True(result.ContainsKey("RealField"));
      Assert.IsAssignableFrom<Single>(result["RealField"]);

      Assert.True(result.ContainsKey("DoublePrecisionField"));
      Assert.IsAssignableFrom<Double>(result["DoublePrecisionField"]);

      Assert.True(result.ContainsKey("SerialField"));
      Assert.IsAssignableFrom<Int32>(result["SerialField"]);

      Assert.True(result.ContainsKey("BigserialField"));
      Assert.IsAssignableFrom<Int64>(result["BigserialField"]);

      Assert.True(result.ContainsKey("MoneyField"));
      Assert.IsAssignableFrom<Decimal>(result["MoneyField"]);

      Assert.True(result.ContainsKey("CharacterVaryingUnlimitedField"));
      Assert.IsAssignableFrom<String>(result["CharacterVaryingUnlimitedField"]);

      Assert.True(result.ContainsKey("CharacterVarying30Field"));
      Assert.IsAssignableFrom<String>(result["CharacterVarying30Field"]);

      Assert.True(result.ContainsKey("VarcharUnlimitedField"));
      Assert.IsAssignableFrom<String>(result["VarcharUnlimitedField"]);

      Assert.True(result.ContainsKey("Varchar30Field"));
      Assert.IsAssignableFrom<String>(result["Varchar30Field"]);

      Assert.True(result.ContainsKey("CharacterField"));
      Assert.IsAssignableFrom<String>(result["CharacterField"]);

      Assert.True(result.ContainsKey("Character10Field"));
      Assert.IsAssignableFrom<String>(result["Character10Field"]);

      Assert.True(result.ContainsKey("CharField"));
      Assert.IsAssignableFrom<String>(result["CharField"]);

      Assert.True(result.ContainsKey("Char10Field"));
      Assert.IsAssignableFrom<String>(result["Char10Field"]);

      Assert.True(result.ContainsKey("TextField"));
      Assert.IsAssignableFrom<String>(result["TextField"]);

      Assert.True(result.ContainsKey("ByteaField"));
      Assert.IsAssignableFrom<Byte[]>(result["ByteaField"]);

      Assert.True(result.ContainsKey("TimestampField"));
      Assert.IsAssignableFrom<DateTime>(result["TimestampField"]);

      Assert.True(result.ContainsKey("TimestampWithoutTimeZoneField"));
      Assert.IsAssignableFrom<DateTime>(result["TimestampWithoutTimeZoneField"]);

      Assert.True(result.ContainsKey("TimestamptzField"));
      Assert.IsAssignableFrom<DateTime>(result["TimestamptzField"]);

      Assert.True(result.ContainsKey("TimestampWithTimeZoneField"));
      Assert.IsAssignableFrom<DateTime>(result["TimestampWithTimeZoneField"]);

      Assert.True(result.ContainsKey("DateField"));
      Assert.IsAssignableFrom<DateTime>(result["DateField"]);

      Assert.True(result.ContainsKey("TimeField"));
      Assert.IsAssignableFrom<DateTime>(result["TimeField"]);

      Assert.True(result.ContainsKey("TimeWithoutTimeZoneField"));
      Assert.IsAssignableFrom<DateTime>(result["TimeWithoutTimeZoneField"]);

      Assert.True(result.ContainsKey("TimetzField"));
      Assert.IsAssignableFrom<DateTime>(result["TimetzField"]);

      Assert.True(result.ContainsKey("TimeWithTimeZoneField"));
      Assert.IsAssignableFrom<DateTime>(result["TimeWithTimeZoneField"]);

      Assert.True(result.ContainsKey("IntervalField"));
      Assert.IsAssignableFrom<TimeSpan>(result["IntervalField"]);

      Assert.True(result.ContainsKey("BooleanField"));
      Assert.IsAssignableFrom<Boolean>(result["BooleanField"]);

      Assert.True(result.ContainsKey("PointField"));
      Assert.IsAssignableFrom<NpgsqlPoint>(result["PointField"]);

      Assert.True(result.ContainsKey("LsegField"));
      Assert.IsAssignableFrom<NpgsqlLSeg>(result["LsegField"]);

      Assert.True(result.ContainsKey("BoxField"));
      Assert.IsAssignableFrom<NpgsqlBox>(result["BoxField"]);

      Assert.True(result.ContainsKey("PathClosedField"));
      Assert.IsAssignableFrom<NpgsqlPath>(result["PathClosedField"]);

      Assert.True(result.ContainsKey("PathOpenField"));
      Assert.IsAssignableFrom<NpgsqlPath>(result["PathOpenField"]);

      Assert.True(result.ContainsKey("PolygonField"));
      Assert.IsAssignableFrom<NpgsqlPolygon>(result["PolygonField"]);

      Assert.True(result.ContainsKey("CircleField"));
      Assert.IsAssignableFrom<NpgsqlCircle>(result["CircleField"]);

      Assert.True(result.ContainsKey("CidrField"));
      Assert.IsAssignableFrom<String>(result["CidrField"]);

      Assert.True(result.ContainsKey("InetField"));
      Assert.IsAssignableFrom<System.Net.IPAddress>(result["InetField"]);

      Assert.True(result.ContainsKey("MacaddrField"));
      Assert.IsAssignableFrom<String>(result["MacaddrField"]);

      // bit(1) is a special case.  Actual CLR type is BitString, but converts in the Npgsql driver to Boolean.
      Assert.True(result.ContainsKey("BitField"));
      Assert.IsAssignableFrom<Boolean>(result["BitField"]);

      Assert.True(result.ContainsKey("Bit10Field"));
      Assert.IsAssignableFrom<BitString>(result["Bit10Field"]);

      Assert.True(result.ContainsKey("BitVaryingUnlimitedField"));
      Assert.IsAssignableFrom<String>(result["BitVaryingUnlimitedField"]);

      Assert.True(result.ContainsKey("BitVarying10Field"));
      Assert.IsAssignableFrom<String>(result["BitVarying10Field"]);

      Assert.True(result.ContainsKey("TsvectorField"));
      Assert.IsAssignableFrom<String>(result["TsvectorField"]);

      Assert.True(result.ContainsKey("TsqueryField"));
      Assert.IsAssignableFrom<String>(result["TsqueryField"]);

      Assert.True(result.ContainsKey("UuidField"));
      Assert.IsAssignableFrom<Guid>(result["UuidField"]);

      Assert.True(result.ContainsKey("OidField"));
      Assert.IsAssignableFrom<Int64>(result["OidField"]);
    }

    [Test]
    public void FindAllArrayTypes()
    {
      var db = Database.Open();
      var result = db.ArrayTypes.FindById(1) as IDictionary<string, object>;

      Assert.NotNull(result);

      Assert.True(result.ContainsKey("IntegerArrayField"));
      Assert.IsAssignableFrom<Int32[]>(result["IntegerArrayField"]);

      Assert.True(result.ContainsKey("RealArrayField"));
      Assert.IsAssignableFrom<Single[]>(result["RealArrayField"]);

      Assert.True(result.ContainsKey("DoublePrecisionArrayField"));
      Assert.IsAssignableFrom<Double[]>(result["DoublePrecisionArrayField"]);

      Assert.True(result.ContainsKey("VarcharArrayField"));
      Assert.IsAssignableFrom<String[]>(result["VarcharArrayField"]);

      Assert.True(result.ContainsKey("IntegerMultiArrayField"));
      Assert.IsAssignableFrom<Int32[,]>(result["IntegerMultiArrayField"]);

      Assert.True(result.ContainsKey("RealMultiArrayField"));
      Assert.IsAssignableFrom<Single[,]>(result["RealMultiArrayField"]);

      Assert.True(result.ContainsKey("DoublePrecisionMultiArrayField"));
      Assert.IsAssignableFrom<Double[,]>(result["DoublePrecisionMultiArrayField"]);

      Assert.True(result.ContainsKey("VarcharMultiArrayField"));
      Assert.IsAssignableFrom<String[,]>(result["VarcharMultiArrayField"]);

    }
  }
}
