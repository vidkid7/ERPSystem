using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.Inventory;

public class StockTransfer : BranchAwareEntity
{
    public string TransferNumber { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public int FromGodownId { get; set; }
    public Godown FromGodown { get; set; } = null!;
    public int ToGodownId { get; set; }
    public Godown ToGodown { get; set; } = null!;
    public int? CostClassId { get; set; }
    public CostClass? CostClass { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
    public bool IsPosted { get; set; }
    public DateTime? PostedDate { get; set; }
}
