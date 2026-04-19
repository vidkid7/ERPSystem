using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetIssue : BaseEntity
{
    public int AssetId { get; set; }
    public AssetMaster Asset { get; set; } = null!;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public string? IssueDateBS { get; set; }
    public string? Remarks { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
}
