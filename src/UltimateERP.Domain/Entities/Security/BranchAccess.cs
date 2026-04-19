using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.Security;

public class BranchAccess : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
}
