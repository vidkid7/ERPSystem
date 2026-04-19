using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Security;

public class EntityPermission : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int EntityId { get; set; }

    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
    public bool CanReport { get; set; }
}
