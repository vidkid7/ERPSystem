using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Security;

public class UserGroupMember : BaseEntity
{
    public int UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
