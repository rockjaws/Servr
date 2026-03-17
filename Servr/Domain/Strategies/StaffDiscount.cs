using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class StaffDiscount : IDiscountable
{
    public string Name => "Staff Discount (20%)";
    public DiscountType DiscountType => DiscountType.Staff;
    public decimal DiscountAmount => 0.80m;

    public decimal ApplyDiscount(decimal totalPrice)
    {
        return totalPrice * DiscountAmount;
    }
}
