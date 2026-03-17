using Servr.Domain.Enum;
using Servr.Domain.Interface;

public class StudentDiscount : IDiscountStrategy
{
    public string Name => "Student Discount (15%)";
    public decimal Apply(decimal subTotal) => subTotal * 0.85m;
}
