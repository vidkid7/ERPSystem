using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesQuotation : BranchAwareEntity
{
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public string? QuotationDateBS { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public SalesQuotationStatus Status { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }

    public ICollection<SalesQuotationDetail> Details { get; set; } = new List<SalesQuotationDetail>();
}
