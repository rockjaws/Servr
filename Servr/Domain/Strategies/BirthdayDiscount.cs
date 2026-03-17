using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class BirthdayDiscount : IDiscountable
{
    public string Name => "Birthday Discount (10%)";
    public DiscountType DiscountType => DiscountType.Birthday;
    public decimal DiscountAmount => 0.90m;

    public decimal ApplyDiscount(decimal totalPrice)
    {
        return totalPrice * DiscountAmount;
    }
}
