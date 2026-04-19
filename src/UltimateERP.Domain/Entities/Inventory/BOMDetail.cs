using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class BOMDetail : BaseEntity
{
    public int BOMId { get; set; }
    public BOM BOM { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ComponentProductId { get; set; }
    public Product ComponentProduct { get; set; } = null!;
    public decimal Quantity { get; set; }
    public int? UnitId { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}
