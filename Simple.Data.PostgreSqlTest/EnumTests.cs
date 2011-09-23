using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Simple.Data.PostgreSqlTest
{
  public class EnumTest : IDisposable
  {
    public EnumTest()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void ConvertsBetweenEnumAndInt()
    {
      var db = Database.Open();
      EnumTestClass actual = db.EnumTest.Insert(Flag: TestFlag.One);
      Assert.Equal(TestFlag.One, actual.Flag);

      actual.Flag = TestFlag.Three;

      db.EnumTest.Update(actual);

      actual = db.EnumTest.FindById(actual.Id);
      Assert.Equal(TestFlag.Three, actual.Flag);
    }
  }

  class EnumTestClass
  {
    public int Id { get; set; }
    public TestFlag Flag { get; set; }
  }

  enum TestFlag
  {
    None = 0,
    One = 1,
    Two = 2,
    Three = 3
  }
}
