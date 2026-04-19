using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class PurchaseCreditNote : BranchAwareEntity
{
    public string NoteNumber { get; set; } = string.Empty;
    public DateTime NoteDate { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public string? InvoiceReference { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public PurchaseDocumentStatus Status { get; set; }
}
