using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class DispatchOrder : BranchAwareEntity
{
    public string DispatchNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; }
    public string? DispatchDateBS { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public int? CostClassId { get; set; }
    public DispatchStatus Status { get; set; }

    public ICollection<DispatchSection> Sections { get; set; } = new List<DispatchSection>();
}
