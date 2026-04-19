using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Inventory;

public class Product : BaseEntity
{
    public int? ProductTypeId { get; set; }
    public int? ProductGroupId { get; set; }
    public ProductGroup? ProductGroup { get; set; }
    public int? ProductCategoryId { get; set; }
    public int? BrandId { get; set; }
    public int? CompanyId { get; set; }
    public int? DivisionId { get; set; }
    public int? ColorId { get; set; }
    public int? FlavourId { get; set; }
    public int? ShapeId { get; set; }
    public int? UnitId { get; set; }
    public CostingMethod CostingMethod { get; set; }
    public decimal StandardCost { get; set; }
    public decimal MRP { get; set; }
    public decimal WholesaleRate { get; set; }
    public decimal RetailRate { get; set; }
    public decimal OpeningStock { get; set; }
    public decimal OpeningValue { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal MaximumLevel { get; set; }
    public bool IsBatchTracked { get; set; }
    public bool IsExpiryTracked { get; set; }
    public bool IsSerialTracked { get; set; }
    public string? HSNCode { get; set; }
    public decimal TaxRate { get; set; }

    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
