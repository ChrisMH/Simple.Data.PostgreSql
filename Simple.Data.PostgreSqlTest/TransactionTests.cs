using System;
using Simple.Data.PostgreSqlTest;
using Xunit;

namespace Simple.Data.SqlTest
{
  public class TransactionTests
  {
    public TransactionTests()
    {
      DatabaseUtility.CreateDatabase("Test");
    }

    public void Dispose()
    {
      DatabaseUtility.DestroyDatabase("Test");
    }


    [Fact]
    public void TestCommit()
    {
      var db = Database.Open();

      using (var tx = db.BeginTransaction())
      {
        try
        {
          var order = tx.Orders.Insert(CustomerId: 1, OrderDate: DateTime.Today);
          tx.OrderItems.Insert(OrderId: order.OrderId, ItemId: 1, Quantity: 3);
          tx.Commit();
        }
        catch
        {
          tx.Rollback();
          throw;
        }
      }
      Assert.Equal(2, db.Orders.All().ToList().Count);
      Assert.Equal(2, db.OrderItems.All().ToList().Count);
    }

    [Fact]
    public void TestRollback()
    {
      var db = Database.Open();

      using (var tx = db.BeginTransaction())
      {
        var order = tx.Orders.Insert(CustomerId: 1, OrderDate: DateTime.Today);
        tx.OrderItems.Insert(OrderId: order.OrderId, ItemId: 1, Quantity: 3);
        tx.Rollback();
      }
      Assert.Equal(1, db.Orders.All().ToList().Count);
      Assert.Equal(1, db.OrderItems.All().ToList().Count);
    }

    [Fact]
    public void QueryInsideTransaction()
    {
      var db = Database.Open();

      using (var tx = db.BeginTransaction())
      {
        tx.Users.Insert(Name: "Arthur", Age: 42, Password: "Ladida");
        User u2 = tx.Users.FindByName("Arthur");
        Assert.NotNull(u2);
        Assert.Equal("Arthur", u2.Name);
      }
    }
  }
}