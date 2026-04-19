using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class Godown : BaseEntity
{
    public int? ParentGodownId { get; set; }
    public Godown? ParentGodown { get; set; }
    public string? Address { get; set; }
    public string? GodownType { get; set; }

    public ICollection<Godown> ChildGodowns { get; set; } = new List<Godown>();
    public ICollection<Rack> Racks { get; set; } = new List<Rack>();
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
