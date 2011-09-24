using System;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class NaturalJoinTest
  {
    public NaturalJoinTest()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void CustomerDotOrdersDotOrderDateShouldReturnOneRow()
    {
      var db = Database.Open();
      var row = db.Customers.Find(db.Customers.Orders.OrderDate == new DateTime(2010, 10, 10));
      Assert.NotNull(row);
      Assert.Equal("Test", row.Name);
    }

    [Fact]
    public void CustomerDotOrdersDotOrderItemsDotItemDotNameShouldReturnOneRow()
    {
      var db = Database.Open();
      var customer = db.Customers.Find(db.Customers.Orders.OrderItems.Item.Name == "Widget");
      Assert.NotNull(customer);
      Assert.Equal("Test", customer.Name);
      foreach (var order in customer.Orders)
      {
        Assert.Equal(1, order.Id);
      }
    }
  }
}