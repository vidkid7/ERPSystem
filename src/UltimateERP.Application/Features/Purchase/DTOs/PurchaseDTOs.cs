namespace UltimateERP.Application.Features.Purchase.DTOs;

public class PurchaseInvoiceDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public int GodownId { get; set; }
    public string? GodownName { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<PurchaseInvoiceDetailDto> Details { get; set; } = new();
}

public class PurchaseInvoiceDetailDto
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

public class CreatePurchaseInvoiceDto
{
    public DateTime InvoiceDate { get; set; }
    public int VendorId { get; set; }
    public int GodownId { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
    public List<CreatePurchaseDetailDto> Details { get; set; } = new();
}

public class CreatePurchaseDetailDto
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxPercent { get; set; }
}
