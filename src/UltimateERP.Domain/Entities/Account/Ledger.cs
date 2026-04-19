using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class Ledger : BaseEntity
{
    public int LedgerGroupId { get; set; }
    public LedgerGroup LedgerGroup { get; set; } = null!;

    public decimal OpeningBalance { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal ClosingBalance { get; set; }
    public LedgerType LedgerType { get; set; }

    public int? CategoryId { get; set; }
    public int? ChannelId { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
    public string? PANNumber { get; set; }
    public string? VATNumber { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsBillWise { get; set; }
    public int? CostCenterId { get; set; }

    public Customer? Customer { get; set; }
    public Vendor? Vendor { get; set; }
    public ICollection<VoucherDetail> VoucherDetails { get; set; } = new List<VoucherDetail>();
}
