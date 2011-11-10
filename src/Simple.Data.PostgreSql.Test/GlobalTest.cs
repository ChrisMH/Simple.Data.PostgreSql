using System;
using System.Diagnostics;
using System.Xml.Linq;
using NUnit.Framework;
using Utility.Database;
using Utility.Database.PostgreSql;

namespace Simple.Data.PostgreSql.Test
{
  /// <summary>
  /// Sets up and tears down for the test assembly.
  /// Leave this class outside of any namespace so that it applies to any namespace in the assembly
  /// </summary>
  [SetUpFixture]
  public static class GlobalTest
  {
    public static IDbCreator Database { get; private set; }

    static GlobalTest()
    {
      try
      {
        Database = new PgCreator(new DbDescription(XElement.Parse(Properties.Resources.TestDbDescription)));
      }
      catch (Exception e)
      {
        Debug.WriteLine("GlobalTest.GlobalTest : {0} : {1}", e.GetType(), e.Message);
        throw;
      }
    }

    [SetUp]
    public static void SetUp()
    {
      try
      {
        Database.Create();
      }
      catch (Exception e)
      {
        Debug.WriteLine("GlobalTest.Setup : {0} : {1}", e.GetType(), e.Message);
        throw;
      }
    }

    [TearDown]
    public static void TearDown()
    {
      try
      {
        Database.Destroy();
      }
      catch (Exception e)
      {
        Debug.WriteLine("GlobalTest.Setup : {0} : {1}", e.GetType(), e.Message);
        throw;
      }
    }
  }
}