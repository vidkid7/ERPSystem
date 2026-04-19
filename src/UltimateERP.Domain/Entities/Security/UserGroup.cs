using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Security;

public class UserGroup : BaseEntity
{
    public string GroupName { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<UserGroupMember> Members { get; set; } = new List<UserGroupMember>();
}
