using Servr.Domain.Enum;

namespace Servr.Domain.Interface;

public interface IDiscountable
{
    DiscountType DiscountType { get; set; }
    decimal DiscountAmount { get; }
}
