using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class LetterOfCredit : BaseEntity
{
    public string LCNumber { get; set; } = string.Empty;
    public DateTime OpeningDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public decimal AmountInFC { get; set; }
    public int? LCCurrencyId { get; set; }
    public int? BankId { get; set; }
    public int? VendorId { get; set; }
    public Vendor? Vendor { get; set; }
    public string? ShipmentTerms { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
    public LCStatus Status { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
}
