using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.Inventory;

public class PurchaseInvoice : BranchAwareEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public string? ReferenceNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public int? PaymentTermsId { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }

    public ICollection<PurchaseInvoiceDetail> Details { get; set; } = new List<PurchaseInvoiceDetail>();
}
