namespace UltimateERP.Application.Features.Assets;

public class AssetGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class AssetTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? AssetGroupId { get; set; }
    public string? AssetGroupName { get; set; }
    public decimal DepreciationRate { get; set; }
    public int UsefulLifeYears { get; set; }
    public string? DepreciationMethod { get; set; }
    public bool IsActive { get; set; }
}

public class AssetModelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? AssetTypeId { get; set; }
    public string? AssetTypeName { get; set; }
    public string? Manufacturer { get; set; }
    public string? Specifications { get; set; }
    public bool IsActive { get; set; }
}

public class AssetItemDto
{
    public int Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int? AssetModelId { get; set; }
    public string? AssetModelName { get; set; }
    public int? AssetCategoryId { get; set; }
    public string? AssetCategoryName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseCost { get; set; }
    public decimal CurrentValue { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public string? Status { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public string? AssignedToEmployeeName { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}

public class AssetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public bool IsActive { get; set; }
}

public class AssetTransactionDto
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public string? AssetCode { get; set; }
    public string? AssetName { get; set; }
    public string? TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public int? FromEmployeeId { get; set; }
    public string? FromEmployeeName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string? ToEmployeeName { get; set; }
    public string? Remarks { get; set; }
    public decimal Amount { get; set; }
    public string? DocumentNo { get; set; }
}
