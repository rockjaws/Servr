using Servr.Domain.Enum;
using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class Order : IOrder
{
  public int OrderId { get; }
  public int Table { get; }
  public OrderStatus Status { get; private set; }
  public List<IDrink> Drinks { get; }
  public List<IMenuItem> Food { get; }

  public Order(int orderId, int table, IEnumerable<IItem> items)
  {
    OrderId = orderId;
    Status = OrderStatus.Received;
    Table = table;
    Drinks = items.OfType<IDrink>().ToList();
    Food = items.OfType<IMenuItem>().ToList();
  }

  public void UpdateOrderStatus(OrderStatus status)
  {
    Status = status;
  }
}
