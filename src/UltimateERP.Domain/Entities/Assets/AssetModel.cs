using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetModel : BaseEntity
{
    public int? AssetTypeId { get; set; }
    public AssetType? AssetType { get; set; }
    public string? Manufacturer { get; set; }
    public string? Specifications { get; set; }
    public ICollection<Asset> Assets { get; set; } = [];
}
