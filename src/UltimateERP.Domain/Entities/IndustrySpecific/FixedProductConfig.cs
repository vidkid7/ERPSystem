using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.IndustrySpecific;

public class FixedProductConfig : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string? ChassisNumber { get; set; }
    public string? EngineNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public int? ModelYear { get; set; }
    public string? Color { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
    public DateTime? SalesDate { get; set; }
    public decimal SalesValue { get; set; }
    public FixedProductStatus CurrentStatus { get; set; }
    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }
}
