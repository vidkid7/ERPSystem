namespace UltimateERP.Application.Features.Sales.DTOs;

public class SalesInvoiceDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int GodownId { get; set; }
    public string? GodownName { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<SalesInvoiceDetailDto> Details { get; set; } = new();
}

public class SalesInvoiceDetailDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxPercent { get; set; }
}

public class CreateSalesInvoiceDto
{
    public DateTime InvoiceDate { get; set; }
    public int CustomerId { get; set; }
    public int GodownId { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
    public List<CreateSalesDetailDto> Details { get; set; } = new();
}

public class CreateSalesDetailDto
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxPercent { get; set; }
}
