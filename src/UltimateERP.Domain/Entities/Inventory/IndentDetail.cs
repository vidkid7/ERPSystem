using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class IndentDetail : BaseEntity
{
    public int IndentId { get; set; }
    public Indent Indent { get; set; } = null!;
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal RequestedQuantity { get; set; }
    public decimal ApprovedQuantity { get; set; }
    public string? Remarks { get; set; }
}
