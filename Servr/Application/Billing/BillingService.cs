using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Model;

namespace Servr.Application.Billing;

public class BillingService
{
    private ILogger _logger;

    private Dictionary<int, IBill> _bills = new();
    public IReadOnlyDictionary<int, IBill> Bills => _bills;
    private int _nextBillId = 1;
    private readonly string _serverName = "Bo";

    public BillingService(ILogger logger)
    {
        _logger = logger;
    }

    public void ClearBill(int table)
    {
        _bills.Remove(table);
        _logger.Log(LogLevel.INFO, $"Bill cleared for table {table}.");
    }

    public void ProcessBill(IBill bill) { }

    public void AddOrderToTable(int tableNumber, DiscountType discount, IOrder order)
    {
        if (_bills.TryGetValue(tableNumber, out var bill))
        {
            bill.Orders.Add(order);
            _logger.Log(LogLevel.INFO, $"Order {order.OrderId} added to existing bill for table {tableNumber}.");
        }
        else
        {
            _bills[tableNumber] = new Bill(
                _nextBillId++,
                discount,
                _serverName,
                new List<IOrder> { order }
            );
            _logger.Log(LogLevel.INFO, $"New bill (ID {_nextBillId - 1}) created for table {tableNumber} with discount '{discount}'.");
        }
    }

    public IBill GetBillForTable(int tableNumber)
    {
        _bills.TryGetValue(tableNumber, out var bill);
        return bill;
    }
}
