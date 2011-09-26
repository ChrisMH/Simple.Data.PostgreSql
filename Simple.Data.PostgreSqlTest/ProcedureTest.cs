using System.Data;
using System.Diagnostics;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class ProcedureTest
  {
    public ProcedureTest()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void GetCustomersTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomers();
      var actual = results.First();
      Assert.Equal(1, actual.Id);
    }
    
    [Fact]
    public void GetCustomerOrdersTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomerOrders(1);
      var actual = results.First();
      Assert.Equal(1, actual.Id);
    }

    [Fact]
    public void GetCustomerCountTest()
    {
      var db = Database.Open();
      var results = db.Public.GetCustomerCount();
      Assert.Equal(1, results.ReturnValue);
    }

    [Fact]
    public void CallOverloadedFunction1()
    {
      var db = Database.Open();
      var results = db.Public.TestOverload(1);
      Assert.Equal(1, results.ReturnValue);
    }

    [Fact]
    public void CallOverloadedFunction2()
    {
      var db = Database.Open();
      var results = db.Public.TestOverload(1, 1);
      Assert.Equal(2, results.ReturnValue);
    }

    //[Fact]
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

    [Fact]
    public void GetCustomerAndOrdersTest()
    {
      var db = Database.Open();
      var results = db.GetCustomerAndOrders(1);
      var customer = results.FirstOrDefault();
      Assert.NotNull(customer);
      Assert.Equal(1, customer.CustomerId);
      Assert.True(results.NextResult());
      var order = results.FirstOrDefault();
      Assert.NotNull(order);
      Assert.Equal(1, order.OrderId);
    }

    [Fact]
    public void GetCustomerAndOrdersStillWorksAfterZeroRecordCallTest()
    {
      var db = Database.Open();
      db.GetCustomerAndOrders(1000);
      var results = db.GetCustomerAndOrders(1);
      var customer = results.FirstOrDefault();
      Assert.NotNull(customer);
      Assert.Equal(1, customer.CustomerId);
      Assert.True(results.NextResult());
      var order = results.FirstOrDefault();
      Assert.NotNull(order);
      Assert.Equal(1, order.OrderId);
    }

    [Fact]
    public void ScalarFunctionIsCalledCorrectly()
    {
      var db = Database.Open();
      var results = db.VarcharAndReturnInt("The answer to everything");
      Assert.Equal(42, results.ReturnValue);
    }

    [Fact]
    public void CallProcedureWithDataTable()
    {
      var db = Database.Open();
      var dataTable = new DataTable();
      dataTable.Columns.Add("Value");
      dataTable.Rows.Add("One");
      dataTable.Rows.Add("Two");
      dataTable.Rows.Add("Three");

      var actual = db.ReturnStrings(dataTable).ToScalarList<string>();

      Assert.Equal(3, actual.Count);
      Assert.Contains("One", actual);
      Assert.Contains("Two", actual);
      Assert.Contains("Three", actual);
    }
  }
}