using System;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class OrderDetailTests
  {
    public OrderDetailTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }

    [Fact]
    public void TestOrderDetail()
    {
      var db = Database.Open();
      var order = db.Orders.FindByOrderDate(new DateTime(2010, 10, 10));
      Assert.NotNull(order);

      var orderItem = order.OrderItems.FirstOrDefault();
      var item = orderItem.Item;
      Assert.NotNull(item);
      Assert.Equal("Widget", item.Name);
    }
  }
}