using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class TeaPurchaseInvoice : BranchAwareEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public string? GardenName { get; set; }
    public string? LotNumber { get; set; }
    public int? GradeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal NetAmount { get; set; }
    public bool IsPosted { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
}
