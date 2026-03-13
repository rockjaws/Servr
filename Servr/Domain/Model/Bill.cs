using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class Bill : IBill
{
    public int BillId { get; }
    public decimal BillTotal { get; private set; }
    public string Server { get; }
    public List<IOrder> Orders { get; }

    public Bill(int billId, decimal billTotal, string server, List<IOrder> orders)
    {
        BillId = billId;
        BillTotal = billTotal;
        Server = server;
        Orders = orders;
    }
}
