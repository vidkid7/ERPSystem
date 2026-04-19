namespace UltimateERP.Domain.Common;

/// <summary>
/// Base class for entities scoped to a specific branch.
/// </summary>
public abstract class BranchAwareEntity : AuditableEntity
{
    public int BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? BranchCode { get; set; }
}
