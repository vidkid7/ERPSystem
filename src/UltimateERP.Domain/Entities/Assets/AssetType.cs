using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetType : BaseEntity
{
    public int? AssetGroupId { get; set; }
    public AssetGroup? AssetGroup { get; set; }
    public decimal DepreciationRate { get; set; }
    public int UsefulLifeYears { get; set; }
    public DepreciationMethod DepreciationMethod { get; set; }
    public ICollection<AssetModel> AssetModels { get; set; } = [];
}
