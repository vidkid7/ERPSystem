using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesQuotationDetail : BaseEntity
{
    public int SalesQuotationId { get; set; }
    public SalesQuotation SalesQuotation { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
}
