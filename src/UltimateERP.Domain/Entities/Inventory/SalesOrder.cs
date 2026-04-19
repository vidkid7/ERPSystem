using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class SalesOrder : BranchAwareEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? OrderDateBS { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int? SalesQuotationId { get; set; }
    public SalesQuotation? SalesQuotation { get; set; }
    public int? GodownId { get; set; }
    public Godown? Godown { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public SalesOrderStatus Status { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Remarks { get; set; }

    public ICollection<SalesOrderDetail> Details { get; set; } = new List<SalesOrderDetail>();
}
