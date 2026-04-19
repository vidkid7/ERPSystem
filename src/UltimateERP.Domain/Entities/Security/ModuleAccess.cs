using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Security;

public class ModuleAccess : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ModuleId { get; set; }
    public bool HasAccess { get; set; }
}
