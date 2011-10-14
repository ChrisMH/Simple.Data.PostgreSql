using System;
using NUnit.Framework;
using Simple.Data.PostgreSql.Test.Utility;

namespace Simple.Data.PostgreSql.Test
{
  public class NaturalJoinTest
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
    public void CustomerDotOrdersDotOrderDateShouldReturnOneRow()
    {
      var db = Database.Open();
      var row = db.Customers.Find(db.Customers.Orders.OrderDate == new DateTime(2010, 10, 10));
      Assert.IsNotNull(row);
      Assert.AreEqual("Test", row.Name);
    }

    [Test]
    public void CustomerDotOrdersDotOrderItemsDotItemDotNameShouldReturnOneRow()
    {
      var db = Database.Open();
      var customer = db.Customers.Find(db.Customers.Orders.OrderItems.Item.Name == "Widget");
      Assert.IsNotNull(customer);
      Assert.AreEqual("Test", customer.Name);
      foreach (var order in customer.Orders)
      {
        Assert.AreEqual(1, order.Id);
      }
    }
  }
}