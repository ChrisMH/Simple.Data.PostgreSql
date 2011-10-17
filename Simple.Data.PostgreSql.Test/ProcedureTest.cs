using System.Data;
using NUnit.Framework;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class ProcedureTest
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
    public void TestNoReturn()
    {
      var db = Database.Open();
      var result = db.Public.TestNoReturn();
      Assert.False(result.NextResult());
    }

    [Test]
    public void TestReturn()
    {
      var db = Database.Open();
      var result = db.Public.TestReturn(2);
      Assert.AreEqual(4, result.ReturnValue);
    }

    [Test]
    public void TestReturnNoParameterNames()
    {
      var db = Database.Open();
      var result = db.Public.TestReturnNoParameterNames(2);
      Assert.AreEqual(4, result.ReturnValue);
    }

    [Test]
    public void TestOut()
    {
      var db = Database.Open();
      var result = db.Public.TestOut(2);

      Assert.True(result.OutputValues.ContainsKey("doubled"));
      Assert.AreEqual(4, result.OutputValues["doubled"]);
    }

    [Test]
    public void TestOutNoParameterNames()
    {
      var db = Database.Open();
      var result = db.Public.TestOutNoParameterNames(2);

      Assert.True(result.OutputValues.ContainsKey("output0"));
      Assert.AreEqual(4, result.OutputValues["output0"]);
      
    }

    [Test]
    public void GetCustomersTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomers();
      var actual = results.First();
      Assert.AreEqual(1, actual.Id);
    }

    [Test]
    public void GetCustomerOrdersTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomerOrders(1);
      var actual = results.First();
      Assert.AreEqual(1, actual.Id);
    }

    [Test]
    public void GetCustomerCountTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomerCount();
      Assert.AreEqual(1, results.ReturnValue);
    }

    //[Test]
    //public void CallOverloadedFunction1()
    //{
    //  var db = Database.Open();
    //  var results = db.Public.TestOverload(1);
    //  Assert.AreEqual(1, results.ReturnValue);
    //}

    //[Test]
    //public void CallOverloadedFunction2()
    //{
    //  var db = Database.Open();
    //  var results = db.Public.TestOverload(1, 1);
    //  Assert.AreEqual(2, results.ReturnValue);
    //}

    //[Test]
    //public void GetCustomerCountSecondCallExecutesNonQueryTest()
    //{
    //  var listener = new TestTraceListener();
    //  Trace.Listeners.Add(listener);
    //  var db = Database.Open();
    //  db.GetCustomerCount();
    //  Assert.False(listener.Output.Contains("ExecuteNonQuery"));
    //  db.GetCustomerCount();
    //  Assert.True(listener.Output.Contains("ExecuteNonQuery"));
    //  Trace.Listeners.Remove(listener);
    //}

    [Test]
    public void GetCustomerAndOrdersTest()
    {
      var db = Database.Open();
      var results = db.GetCustomerAndOrders(1);
      var customer = results.FirstOrDefault();
      Assert.IsNotNull(customer);
      Assert.AreEqual(1, customer.CustomerId);
      Assert.True(results.NextResult());
      var order = results.FirstOrDefault();
      Assert.IsNotNull(order);
      Assert.AreEqual(1, order.OrderId);
    }

    [Test]
    public void GetCustomerAndOrdersStillWorksAfterZeroRecordCallTest()
    {
      var db = Database.Open();
      db.GetCustomerAndOrders(1000);
      var results = db.GetCustomerAndOrders(1);
      var customer = results.FirstOrDefault();
      Assert.IsNotNull(customer);
      Assert.AreEqual(1, customer.CustomerId);
      Assert.True(results.NextResult());
      var order = results.FirstOrDefault();
      Assert.IsNotNull(order);
      Assert.AreEqual(1, order.OrderId);
    }

    [Test]
    public void CallProcedureWithDataTable()
    {
      var db = Database.Open();
      var dataTable = new DataTable();
      dataTable.Columns.Add("Value");
      dataTable.Rows.Add("One");
      dataTable.Rows.Add("Two");
      dataTable.Rows.Add("Three");

      var actual = db.ReturnStrings(dataTable).ToScalarList<string>();

      Assert.AreEqual(3, actual.Count);
      Assert.Contains("One", actual);
      Assert.Contains("Two", actual);
      Assert.Contains("Three", actual);
    }
  }
}