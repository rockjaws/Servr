using Servr.Application.Kitchen;
using Servr.Domain.Interface;

namespace Servr.Application.Order;

public class OrderService
{
    private KitchenAlgorithm _kitchenAlgorithm;
    private ILogger _logger;

    public OrderService(ILogger logger)
    {
        _logger = logger;
        _kitchenAlgorithm = new KitchenAlgorithm(logger);
    }

    public void NewOrder(IOrder order)
    {
        _logger.Log(LogLevel.INFO, "Starting new order");
        _kitchenAlgorithm.NewOrder(order);
    }
}
