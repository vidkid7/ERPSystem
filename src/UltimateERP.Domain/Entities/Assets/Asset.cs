using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Assets;

public class Asset : BaseEntity
{
    public string AssetCode { get; set; } = string.Empty;
    public int? AssetModelId { get; set; }
    public AssetModel? AssetModel { get; set; }
    public int? AssetCategoryId { get; set; }
    public AssetCategory? AssetCategory { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseCost { get; set; }
    public decimal CurrentValue { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public AssetStatus Status { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public Employee? AssignedToEmployee { get; set; }
    public string? Notes { get; set; }
    public ICollection<AssetTransaction> AssetTransactions { get; set; } = [];
}
