using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class BirthdayDiscount : IDiscountStrategy
{
    public string Name => "Birthday Discount (10%)";
    public decimal Apply(decimal subtotal) => subtotal * 0.90m;
}
