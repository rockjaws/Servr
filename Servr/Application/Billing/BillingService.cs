using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Model;

namespace Servr.Application.Billing;

public class BillingService
{
    private ILogger _logger;

    private readonly Dictionary<int, IBill> _bills = new();
    public IReadOnlyDictionary<int, IBill> Bills => _bills;
    private int _nextBillId = 1;
    private readonly string _serverName = "Bo";
    public BillingService(ILogger logger)
    {
        _logger = logger;
    }


    public void ProcessBill(IBill bill) { }
    public void AddOrderToTable(int tableNumber, DiscountType discount, IOrder order)
    {
        if (_bills.TryGetValue(tableNumber, out var bill))
            bill.Orders.Add(order);
        else
            _bills[tableNumber] = new Bill(_nextBillId++, discount, _serverName, new List<IOrder> { order });
    }
    public IBill GetBillForTable(int tableNumber)
    {
        _bills.TryGetValue(tableNumber, out var bill);
        return bill;
    }
}
