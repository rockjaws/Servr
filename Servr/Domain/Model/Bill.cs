using Servr.Domain.Enum;
using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class Bill : IBill
{
    public int BillId { get; }
    public DiscountType DiscountType { get; }
    public decimal BillTotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public string Server { get; }
    public List<IOrder> Orders { get; }

    public Bill(
        int billId,
        DiscountType discountType,
        decimal billTotal,
        string server,
        List<IOrder> orders
    )
    {
        BillId = billId;
        DiscountType = discountType;
        BillTotal = billTotal;
        Server = server;
        Orders = orders;
    }
}
