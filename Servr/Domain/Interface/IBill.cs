namespace Servr.Domain.Interface;

public interface IBill : IDiscountable
{
    int BillId { get; }
    decimal BillTotal { get; }
    string Server { get; }
    List<IOrder> Orders { get; }
}
