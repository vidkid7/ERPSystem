using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class DispatchSection : BaseEntity
{
    public int DispatchOrderId { get; set; }
    public DispatchOrder DispatchOrder { get; set; } = null!;
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Quantity { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
}
