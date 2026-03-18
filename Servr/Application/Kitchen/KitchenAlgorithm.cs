using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Presentation.ViewModel;

namespace Servr.Application.Kitchen;

public sealed class KitchenAlgorithm : ObservableObject, IKitchenObserver
{
    private readonly ILogger _logger;
    private readonly object _lock = new();
    private int _receivedOrders;
    private int _ordersInProgress;
    private int _ordersReady;
    private Queue<IOrder> _queue;
    private Dictionary<IOrder, TimeSpan> _orderTime;

    public Queue<IOrder> Queue
    {
        get => _queue;
        set => SetProperty(ref _queue, value);
    }

    public int ReceivedOrders
    {
        get => _receivedOrders;
        set => SetProperty(ref _receivedOrders, value);
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

    public event Action<IOrder>? OrderReady;

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

        TimeSpan orderTime = GetOrderTime(order);

        lock (_lock)
        {
            ReceivedOrders += 1;

            _logger.Log(
                LogLevel.INFO,
                $"Order: {order.OrderId} order time upon processing {orderTime}"
            );

            _orderTime.Add(order, orderTime);
            Queue.Enqueue(order);
            _logger.Log(
                LogLevel.INFO,
                $"Order: {order.OrderId} is now in queue, waiting for processing.."
            );
        }

        _ = ProcessOrder();
    }

    private async Task ProcessOrder()
    {
        IOrder order;
        TimeSpan orderTime;

        lock (_lock)
        {
            if (_queue.Count <= 0)
                return;
            order = _queue.Dequeue();
            if (!_orderTime.TryGetValue(order, out orderTime))
            {
                _logger.Log(LogLevel.WARNING, $"Order {order.OrderId} dequeued but no time entry found. Skipping.");
                return;
            }
            _orderTime.Remove(order);

            _logger.Log(LogLevel.INFO, $"Order {order.OrderId} for table {order.Table} started preparing. ETA: {orderTime.TotalSeconds}s.");
            OrdersInProgress += 1;
        }

        order.UpdateOrderStatus(OrderStatus.Preparing);
        await Task.Delay(orderTime);
        order.UpdateOrderStatus(OrderStatus.Ready);

        lock (_lock)
        {
            OrdersInProgress -= 1;
            OrdersReady += 1;
            _logger.Log(LogLevel.INFO, $"Order: {order.OrderId} Ready for table {order.Table}");
        }

        OrderReady?.Invoke(order);
    }

    private TimeSpan GetOrderTime(IOrder order)
    {
        int time = 0;
        foreach (IMenuItem item in order.Food)
        {
            time += 10;
        }

        return TimeSpan.FromSeconds(time);
    }
}
