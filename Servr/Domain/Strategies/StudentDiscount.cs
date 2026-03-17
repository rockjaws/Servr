using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class StudentDiscount : IDiscountable
{
    public string Name => "Student Discount (15%)";
    public DiscountType DiscountType => DiscountType.Student;
    public decimal DiscountAmount => 0.85m;

    public decimal ApplyDiscount(decimal totalPrice)
    {
        return totalPrice * DiscountAmount;
    }
}
