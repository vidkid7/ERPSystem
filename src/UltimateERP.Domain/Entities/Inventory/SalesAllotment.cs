using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesAllotment : BaseEntity
{
    public string AllotmentNumber { get; set; } = string.Empty;
    public DateTime AllotmentDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal AllottedQuantity { get; set; }
    public decimal DeliveredQuantity { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public SalesAllotmentStatus Status { get; set; }
}
