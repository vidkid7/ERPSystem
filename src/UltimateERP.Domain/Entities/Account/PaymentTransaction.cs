using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class PaymentTransaction : BaseEntity
{
    public string TransactionId { get; set; } = string.Empty;
    public string? ReferenceId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; } = "NPR";
    public string? PaymentMethod { get; set; }
    public string? ProviderName { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ProviderResponse { get; set; }
    public int? CustomerId { get; set; }
    public int? VoucherId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
