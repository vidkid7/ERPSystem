using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class PDC : BaseEntity
{
    public int VoucherId { get; set; }
    public Voucher Voucher { get; set; } = null!;

    public string ChequeNumber { get; set; } = string.Empty;
    public DateTime ChequeDate { get; set; }
    public string? ChequeDateBS { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public decimal Amount { get; set; }
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;
    public PDCStatus Status { get; set; }
    public DateTime? ClearedDate { get; set; }
}
