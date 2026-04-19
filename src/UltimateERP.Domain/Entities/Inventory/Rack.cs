using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class Rack : BaseEntity
{
    public int GodownId { get; set; }
    public Godown Godown { get; set; } = null!;
    public string? Location { get; set; }
}
