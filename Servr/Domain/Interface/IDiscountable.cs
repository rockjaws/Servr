using Servr.Domain.Enum;

namespace Servr.Domain.Interface;

public interface IDiscountable
{
    DiscountType DiscountType { get; }
    decimal DiscountAmount { get; }
}
