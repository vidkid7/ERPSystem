using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class GatePass : BranchAwareEntity
{
    public string GatePassNumber { get; set; } = string.Empty;
    public DateTime GatePassDate { get; set; }
    public string? GatePassDateBS { get; set; }
    public GatePassType GatePassType { get; set; }
    public string? PartyName { get; set; }
    public string? VehicleNumber { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public bool IsApproved { get; set; }
    public int? ApprovedBy { get; set; }
}
