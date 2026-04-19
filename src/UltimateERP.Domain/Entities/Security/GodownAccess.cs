using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Domain.Entities.Security;

public class GodownAccess : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int GodownId { get; set; }
    public Godown Godown { get; set; } = null!;
}
