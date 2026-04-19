using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetMaster : BaseEntity
{
    public string AssetCode { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public int? AssetTypeId { get; set; }
    public int? AssetGroupId { get; set; }
    public int? AssetCategoryId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
    public int? VendorId { get; set; }
    public Vendor? Vendor { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public AssetStatus Status { get; set; }
}
