using Servr.Domain.Enum;
using Servr.Domain.Interface;

namespace Servr.Domain.Strategies;

public static class DiscountStrategyFactory
{
    public static IDiscountStrategy? Get(DiscountType type) => type switch
    {
        DiscountType.Student => new StudentDiscount(),
        DiscountType.Staff => new StaffDiscount(),
        DiscountType.Birthday => new BirthdayDiscount(),
        _ => null
    };
}
