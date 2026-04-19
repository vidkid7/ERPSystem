using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class StockDemand : BranchAwareEntity
{
    public string DemandNumber { get; set; } = string.Empty;
    public DateTime DemandDate { get; set; }
    public int? JobCardId { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public int? CostClassId { get; set; }
    public StockDemandStatus Status { get; set; }
}
