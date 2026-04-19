using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class ODC : BaseEntity
{
    public int VoucherId { get; set; }
    public Voucher Voucher { get; set; } = null!;

    public string ChequeNumber { get; set; } = string.Empty;
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;
    public string? Status { get; set; }
}
