namespace UltimateERP.Application.Features.Dispatch.DTOs;

public class DispatchOrderDto
{
    public int Id { get; set; }
    public string DispatchNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; }
    public string? DispatchDateBS { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
    public List<DispatchSectionDto> Sections { get; set; } = new();
}

public class DispatchSectionDto
{
    public int Id { get; set; }
    public int DispatchOrderId { get; set; }
    public int? SalesInvoiceId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public int? GodownId { get; set; }
}

public class CreateDispatchOrderDto
{
    public string DispatchNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; }
    public string? DispatchDateBS { get; set; }
    public int CustomerId { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public int BranchId { get; set; }
    public List<CreateDispatchSectionDto> Sections { get; set; } = new();
}

public class CreateDispatchSectionDto
{
    public int? SalesInvoiceId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public int? GodownId { get; set; }
}

public class ApproveDispatchDto
{
    public int DispatchOrderId { get; set; }
}
