using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class VoucherDetail : BaseEntity
{
    public int VoucherId { get; set; }
    public Voucher Voucher { get; set; } = null!;

    public int LineNumber { get; set; }
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Narration { get; set; }
    public int? CostCenterId { get; set; }
    public int? CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public int? ProjectId { get; set; }
}
