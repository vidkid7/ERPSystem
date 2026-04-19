using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class Indent : BranchAwareEntity
{
    public string IndentNumber { get; set; } = string.Empty;
    public DateTime IndentDate { get; set; }
    public string? IndentDateBS { get; set; }
    public int? RequestedByEmployeeId { get; set; }
    public Employee? RequestedByEmployee { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public string? Remarks { get; set; }
    public IndentStatus Status { get; set; }

    public ICollection<IndentDetail> Details { get; set; } = new List<IndentDetail>();
}
