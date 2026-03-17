using Servr.Application.Kitchen;
using Servr.Domain.Interface;

namespace Servr.Application.Order;

public class OrderService
{
    private KitchenAlgorithm _kitchenAlgorithm;

    public OrderService(ILogger logger)
    {
        _kitchenAlgorithm = new KitchenAlgorithm(logger);
    }

    public void NewOrder(IOrder order)
    {
        _kitchenAlgorithm.NewOrder(order);
    }
}
