using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Presentation.ViewModel;

namespace Servr.Application.Kitchen;

public sealed class KitchenAlgorithm : ObservableObject, IKitchenObserver
{
    private readonly ILogger _logger;
    private int _recievedOrders;
    private int _ordersInProgress;
    private int _ordersReady;
    private Queue<IOrder> _queue;
    private Dictionary<IOrder, TimeSpan> _orderTime;

    public Queue<IOrder> Queue
    {
        get => _queue;
        set => SetProperty(ref _queue, value);
    }

    public int RecievedOrders
    {
        get => _recievedOrders;
        set => SetProperty(ref _recievedOrders, value);
    }

    public int OrdersInProgress
    {
        get => _ordersInProgress;
        set => SetProperty(ref _ordersInProgress, value);
    }

    public int OrdersReady
    {
        get => _ordersReady;
        set => SetProperty(ref _ordersReady, value);
    }

    public KitchenAlgorithm(ILogger logger)
    {
        _logger = logger;
        _queue = new Queue<IOrder>();
        _orderTime = new Dictionary<IOrder, TimeSpan>();
    }

    public void Update(IOrder order)
    {
        if (order == null || order.Food.Count <= 0)
            return;

        RecievedOrders += 1;

        TimeSpan? orderTime = GetOrderTime(order);

        if (orderTime == null)
            return;

        _logger.Log(
            LogLevel.INFO,
            $"Order: {order.OrderId} order time upon processing {orderTime.Value}"
        );

        _orderTime.Add(order, orderTime.Value);
        Queue.Enqueue(order);
        _logger.Log(
            LogLevel.INFO,
            $"Order: {order.OrderId} is now in queue, waiting for processing.."
        );

        _ = ProcessOrder();
    }

    private async Task ProcessOrder()
    {
        if (_queue.Count <= 0)
            return;
        IOrder order = _queue.Dequeue();
        if (!_orderTime.TryGetValue(order, out TimeSpan orderTime))
            return;

        order.UpdateOrderStatus(OrderStatus.Preparing);
        await Task.Delay(orderTime);
        order.UpdateOrderStatus(OrderStatus.Ready);
        _logger.Log(LogLevel.INFO, $"Order: {order.OrderId} Ready for table {order.Table}");
    }

    private TimeSpan? GetOrderTime(IOrder order)
    {
        int time = 0;
        foreach (IMenuItem item in order.Food)
        {
            time += 10;
        }

        return TimeSpan.FromSeconds(time);
    }
}
