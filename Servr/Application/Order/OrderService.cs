using Servr.Application.Kitchen;
using Servr.Domain.Interface;

namespace Servr.Application.Order;

public class OrderService : IOrderSubject
{
    private readonly ILogger _logger;
    private readonly List<IKitchenObserver> _observers = new();

    public OrderService(ILogger logger)
    {
        _logger = logger;
    }

    public void Subscribe(IKitchenObserver observer)
    {
        _observers.Add(observer);
    }

    public void Unsubscribe(IKitchenObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(IOrder order)
    {
        foreach (var observer in _observers)
        {
            observer.Update(order);
        }
    }

    public void NewOrder(IOrder order)
    {
        _logger.Log(LogLevel.INFO, "Starting new order");
        NotifyObservers(order);
    }
}
