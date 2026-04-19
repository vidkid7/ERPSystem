using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetCategory : BaseEntity
{
    public int? ParentCategoryId { get; set; }
    public AssetCategory? ParentCategory { get; set; }
    public ICollection<AssetCategory> ChildCategories { get; set; } = [];
    public ICollection<Asset> Assets { get; set; } = [];
}
