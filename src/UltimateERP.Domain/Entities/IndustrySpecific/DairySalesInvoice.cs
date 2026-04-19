using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class DairySalesInvoice : BranchAwareEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public decimal TotalQuantityLitre { get; set; }
    public decimal NetAmount { get; set; }
    public bool IsPosted { get; set; }
    public int? VoucherId { get; set; }
    public Voucher? Voucher { get; set; }
}
