using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class PurchaseQuotation : BranchAwareEntity
{
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public PurchaseDocumentStatus Status { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }

    public ICollection<PurchaseQuotationDetail> Details { get; set; } = new List<PurchaseQuotationDetail>();
}

public class PurchaseQuotationDetail : BaseEntity
{
    public int PurchaseQuotationId { get; set; }
    public PurchaseQuotation PurchaseQuotation { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}
