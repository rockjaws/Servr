namespace Servr.Domain.Interface;

public interface IBill
{
    int BillId { get; }
    decimal BillTotal { get; }
    string Server { get; }
    List<IOrder> Orders { get; }
}
