using System;
using System.Diagnostics;
using System.Xml.Linq;
using NUnit.Framework;
using Utility.Database.Management;
using Utility.Database.Management.PostgreSql;

namespace Simple.Data.PostgreSql.Test
{
  /// <summary>
  /// Sets up and tears down for the test assembly.
  /// Leave this class outside of any namespace so that it applies to any namespace in the assembly
  /// </summary>
  [SetUpFixture]
  public class GlobalTest
  {
    public static IDbManager Database { get; private set; }

    [SetUp]
    public void SetUp()
    {
      try
      {
        Database = new PgDbManager(new PgDbDescription(XElement.Parse(Resources.TestDbDescription)));
        Database.Create();
      }
      catch (Exception e)
      {
        Debug.WriteLine("GlobalTest.Setup : {0} : {1}", e.GetType(), e.Message);
        throw;
      }
    }

    [TearDown]
    public void TearDown()
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