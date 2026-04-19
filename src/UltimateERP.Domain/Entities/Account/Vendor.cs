using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class Vendor : BaseEntity
{
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public string? PANNumber { get; set; }
    public string? VATNumber { get; set; }
    public int? PaymentTermsId { get; set; }
}
