using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetGroup : BaseEntity
{
    public string? Description { get; set; }
    public ICollection<AssetType> AssetTypes { get; set; } = [];
}
