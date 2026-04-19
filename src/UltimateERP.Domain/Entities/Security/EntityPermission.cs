using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Security;

public class EntityPermission : BaseEntity
{
    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }

    public int EntityId { get; set; }

    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
    public bool CanReport { get; set; }
}
