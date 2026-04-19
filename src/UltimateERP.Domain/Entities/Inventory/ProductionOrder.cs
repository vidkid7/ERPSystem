using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class ProductionOrder : BranchAwareEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? OrderDateBS { get; set; }
    public int FinishedProductId { get; set; }
    public Product FinishedProduct { get; set; } = null!;
    public int? BOMId { get; set; }
    public BOM? BOM { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public ProductionOrderStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}
