using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Inventory;

public class ProductGroup : BaseEntity
{
    public int? ParentGroupId { get; set; }
    public ProductGroup? ParentGroup { get; set; }

    public ICollection<ProductGroup> ChildGroups { get; set; } = new List<ProductGroup>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
