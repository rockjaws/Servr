namespace Servr.Domain.Interface;

public interface IOrderSubject
{
    void Subscribe(IKitchenObserver observer);
    void Unsubscribe(IKitchenObserver observer);
    void NotifyObservers(IOrder order);
}
