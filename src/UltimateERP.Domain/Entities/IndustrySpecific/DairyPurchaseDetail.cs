using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class DairyPurchaseDetail : BaseEntity
{
    public int DairyPurchaseInvoiceId { get; set; }
    public DairyPurchaseInvoice DairyPurchaseInvoice { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal FatPercent { get; set; }
    public decimal SNFPercent { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}
