using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesInvoiceDetail : BaseEntity
{
    public int SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; } = null!;
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
    public string? BatchNumber { get; set; }
    public decimal CostAmount { get; set; }
    public decimal ProfitAmount { get; set; }
}
