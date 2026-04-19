namespace UltimateERP.Application.Features.Assets.DTOs;

public class AssetDto
{
    public int Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public int? AssetTypeId { get; set; }
    public int? AssetGroupId { get; set; }
    public int? AssetCategoryId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
}

public class RegisterAssetDto
{
    public string AssetCode { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public int? AssetTypeId { get; set; }
    public int? AssetGroupId { get; set; }
    public int? AssetCategoryId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
    public int? VendorId { get; set; }
    public string? SerialNumber { get; set; }
    public string? Location { get; set; }
}

public class DepreciationResultDto
{
    public int AssetId { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public decimal PurchaseValue { get; set; }
    public decimal AnnualDepreciation { get; set; }
    public decimal AccumulatedDepreciation { get; set; }
    public decimal NetBookValue { get; set; }
}
