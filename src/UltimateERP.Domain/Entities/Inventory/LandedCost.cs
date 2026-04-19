using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class LandedCost : BaseEntity
{
    public int PurchaseInvoiceId { get; set; }
    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
    public string CostType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public LandedCostAllocationType AllocationType { get; set; }
    public string? Description { get; set; }
}
