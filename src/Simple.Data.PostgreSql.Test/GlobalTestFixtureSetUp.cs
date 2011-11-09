
using NUnit.Framework;
using Simple.Data.PostgreSql.Test.Utility;

/// <summary>
/// Sets up and tears down for the test assembly.
/// Leave this class outside of any namespace so that it applies to any namespace in the assembly
/// </summary>
[SetUpFixture]
public class GlobalTestFixtureSetUp
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
}
