using Servr.Domain.Enum;

namespace Servr.Domain.Interface;

public interface IOrder
{
    int OrderId { get; }
    int Table { get; }
    List<IDrink> Drinks { get; }
    List<IMenuItem> Food { get; }
    void UpdateOrderStatus(OrderStatus status);
}
