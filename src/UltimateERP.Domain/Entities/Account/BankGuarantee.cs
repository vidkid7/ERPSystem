using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class BankGuarantee : BaseEntity
{
    public string GuaranteeNumber { get; set; } = string.Empty;
    public decimal GuaranteeAmount { get; set; }
    public string? IssuingBank { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;
    public string? Purpose { get; set; }
    public string? Status { get; set; }
}
