using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class StaffDiscount : IDiscountStrategy
{
    public string Name => "Staff Discount (20%)";
    public decimal Apply(decimal subtotal) => subtotal * 0.80m;
}
