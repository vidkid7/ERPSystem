using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class PaymentTerm : BaseEntity
{
    public int DueDays { get; set; }
    public decimal DiscountPercent { get; set; }
    public string? Description { get; set; }
}
