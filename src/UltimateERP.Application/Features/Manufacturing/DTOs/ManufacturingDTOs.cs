namespace UltimateERP.Application.Features.Manufacturing.DTOs;

public class BOMDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public DateTime BOMDate { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsActive { get; set; }
    public List<BOMDetailDto> Details { get; set; } = new();
}

public class BOMDetailDto
{
    public int Id { get; set; }
    public int BOMId { get; set; }
    public int LineNumber { get; set; }
    public int ComponentProductId { get; set; }
    public string? ComponentProductName { get; set; }
    public decimal Quantity { get; set; }
    public int? UnitId { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

public class CreateBOMDto
{
    public int ProductId { get; set; }
    public DateTime BOMDate { get; set; }
    public decimal TotalCost { get; set; }
    public List<CreateBOMDetailDto> Details { get; set; } = new();
}

public class CreateBOMDetailDto
{
    public int ComponentProductId { get; set; }
    public decimal Quantity { get; set; }
    public int? UnitId { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

public class ProductionOrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? OrderDateBS { get; set; }
    public int FinishedProductId { get; set; }
    public string? FinishedProductName { get; set; }
    public int? BOMId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal ProducedQuantity { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
}

public class CreateProductionOrderDto
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? OrderDateBS { get; set; }
    public int FinishedProductId { get; set; }
    public int? BOMId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public int BranchId { get; set; }
}

public class CompleteProductionOrderDto
{
    public int ProductionOrderId { get; set; }
    public decimal ProducedQuantity { get; set; }
}
