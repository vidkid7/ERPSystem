using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class BOM : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public DateTime BOMDate { get; set; }
    public decimal TotalCost { get; set; }

    public ICollection<BOMDetail> Details { get; set; } = new List<BOMDetail>();
}
