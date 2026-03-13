using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class Order : IOrder
{
    public int OrderId { get; }
    public int Table { get; }
    public List<IDrink> Drinks { get; private set; }
    public List<IMenuItem> Food { get; private set; }

    public Order(int orderId, int table, List<IDrink> drinks, List<IMenuItem> food)
    {
        OrderId = orderId;
        Table = table;
        Drinks = drinks;
        Food = food;
    }
}
