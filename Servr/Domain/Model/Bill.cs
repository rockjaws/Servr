using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Strategies;

namespace Servr.Domain.Model;

public class Bill : IBill
{
    public int BillId { get; }
    public DiscountType DiscountType { get; set; }
    public decimal Subtotal => Orders
        .SelectMany(o => o.Food.Cast<IItem>().Concat(o.Drinks))
        .Sum(i => i.Price);
    public decimal BillTotal => Subtotal - DiscountAmount;
    public decimal DiscountAmount
    {
        get
        {
            var strategy = DiscountStrategyFactory.Get(DiscountType);
            if (strategy == null) return 0m;
            return Subtotal - strategy.Apply(Subtotal);
        }
    }
    public string Server { get; }
    public List<IOrder> Orders { get; }
    public int Table => Orders.FirstOrDefault()?.Table ?? 0;

    public Bill(
        int billId,
        DiscountType discountType,
        string server,
        List<IOrder> orders
    )
    {
        BillId = billId;
        DiscountType = discountType;
        Server = server;
        Orders = orders;
    }
}
