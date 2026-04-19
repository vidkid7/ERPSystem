namespace UltimateERP.Application.Features.Inventory.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ProductGroupId { get; set; }
    public string? ProductGroupName { get; set; }
    public string? Unit { get; set; }
    public string? AlternateUnit { get; set; }
    public decimal ConversionFactor { get; set; }
    public decimal? PurchaseRate { get; set; }
    public decimal? SalesRate { get; set; }
    public decimal? MRP { get; set; }
    public string? HSNCode { get; set; }
    public decimal? TaxRate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ProductGroupId { get; set; }
    public string? Unit { get; set; }
    public string? AlternateUnit { get; set; }
    public decimal ConversionFactor { get; set; } = 1;
    public decimal? PurchaseRate { get; set; }
    public decimal? SalesRate { get; set; }
    public decimal? MRP { get; set; }
    public string? HSNCode { get; set; }
    public decimal? TaxRate { get; set; }
}

public class ProductGroupDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGroupId { get; set; }
    public string? ParentGroupName { get; set; }
    public bool IsActive { get; set; }
    public List<ProductGroupDto> Children { get; set; } = new();
}

public class CreateProductGroupDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGroupId { get; set; }
}

public class GodownDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGodownId { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class CreateGodownDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? ParentGodownId { get; set; }
    public string? Address { get; set; }
}

public class StockDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public int GodownId { get; set; }
    public string? GodownName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Value { get; set; }
}
