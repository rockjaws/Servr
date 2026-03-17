using Servr.Domain.Enum;
using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class Bill : IBill
{
    public int BillId { get; }
    public DiscountType DiscountType { get; }
    public decimal Subtotal => Orders
        .SelectMany(o => o.Food.Cast<IItem>().Concat(o.Drinks))
        .Sum(i => i.Price);
    public decimal BillTotal => Subtotal - DiscountAmount;
    public decimal DiscountAmount { get; private set; }
    public string Server { get; }
    public List<IOrder> Orders { get; }
    public int Table => Orders.First().Table;

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
