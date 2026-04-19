using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.Security;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PasswordSalt { get; set; }
    public string? UserType { get; set; }
    public bool IsSystemAdmin { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? LastLoginIP { get; set; }
    public DateTime? PasswordExpiryDate { get; set; }

    public ICollection<UserGroupMember> UserGroupMembers { get; set; } = new List<UserGroupMember>();
    public ICollection<EntityPermission> EntityPermissions { get; set; } = new List<EntityPermission>();
    public ICollection<ModuleAccess> ModuleAccesses { get; set; } = new List<ModuleAccess>();
    public ICollection<BranchAccess> BranchAccesses { get; set; } = new List<BranchAccess>();
    public ICollection<GodownAccess> GodownAccesses { get; set; } = new List<GodownAccess>();
}
