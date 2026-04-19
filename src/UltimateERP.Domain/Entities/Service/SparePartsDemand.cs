using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Service;

public class SparePartsDemand : BaseEntity
{
    public int JobCardId { get; set; }
    public JobCard JobCard { get; set; } = null!;
    public DateTime DemandDate { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Quantity { get; set; }
    public SparePartStatus Status { get; set; }
}
