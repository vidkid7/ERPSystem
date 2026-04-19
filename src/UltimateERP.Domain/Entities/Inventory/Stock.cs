using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class Stock : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int GodownId { get; set; }
    public Godown Godown { get; set; } = null!;
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Value { get; set; }
    public int? RackId { get; set; }
    public Rack? Rack { get; set; }
}
