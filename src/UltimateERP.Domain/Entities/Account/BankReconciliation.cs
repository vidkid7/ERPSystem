using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class BankReconciliation : BaseEntity
{
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;
    public int? VoucherDetailId { get; set; }
    public VoucherDetail? VoucherDetail { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? BankDate { get; set; }
    public decimal Amount { get; set; }
    public string? Reference { get; set; }
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledDate { get; set; }
}
