namespace Servr.Domain.Interface;

public interface IDiscountStrategy
{
    string Name { get; }
    decimal Apply(decimal subtotal);
}
