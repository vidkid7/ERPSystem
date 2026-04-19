namespace UltimateERP.Application.Features.Sales.DTOs;

// ── Sales Quotation ─────────────────────────────────────────────────

public class SalesQuotationDto
{
    public int Id { get; set; }
    public string QuotationNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public List<SalesQuotationItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Status { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }
}

public class SalesQuotationItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal NetAmount { get; set; }
}

public class CreateSalesQuotationDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? GodownId { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }
    public List<CreateSalesQuotationItemDto> Items { get; set; } = new();
}

public class CreateSalesQuotationItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxPercent { get; set; }
}

// ── Sales Order ─────────────────────────────────────────────────────

public class SalesOrderDto
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? QuotationRef { get; set; }
    public List<SalesOrderItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Status { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Remarks { get; set; }
}

public class SalesOrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Qty { get; set; }
    public decimal DeliveredQty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal NetAmount { get; set; }
}

public class CreateSalesOrderDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? SalesQuotationId { get; set; }
    public int? GodownId { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Remarks { get; set; }
    public List<CreateSalesOrderItemDto> Items { get; set; } = new();
}

public class CreateSalesOrderItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? TaxPercent { get; set; }
}

// ── Sales Allotment ─────────────────────────────────────────────────

public class SalesAllotmentDto
{
    public int Id { get; set; }
    public string AllotmentNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? SalesOrderRef { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal AllocatedQty { get; set; }
    public decimal DeliveredQty { get; set; }
    public string? DeliveryDate { get; set; }
    public string? Status { get; set; }
}

public class CreateSalesAllotmentDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public decimal AllocatedQty { get; set; }
    public int? GodownId { get; set; }
}

// ── Sales Delivery Note (uses DispatchOrder entity) ─────────────────

public class SalesDeliveryNoteDto
{
    public int Id { get; set; }
    public string DeliveryNoteNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? SalesOrderRef { get; set; }
    public List<SalesDeliveryItemDto> Items { get; set; } = new();
    public string? Status { get; set; }
}

public class SalesDeliveryItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal DeliveredQty { get; set; }
}

public class CreateSalesDeliveryNoteDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? SalesInvoiceId { get; set; }
    public int? GodownId { get; set; }
    public List<CreateSalesDeliveryItemDto> Items { get; set; } = new();
}

public class CreateSalesDeliveryItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public int? GodownId { get; set; }
}

// ── Sales Return ────────────────────────────────────────────────────

public class SalesReturnDto
{
    public int Id { get; set; }
    public string ReturnNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? InvoiceRef { get; set; }
    public List<SalesReturnItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
}

public class SalesReturnItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ReturnQty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}

public class CreateSalesReturnDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? SalesInvoiceId { get; set; }
    public int? GodownId { get; set; }
    public string? Reason { get; set; }
    public List<CreateSalesReturnItemDto> Items { get; set; } = new();
}

public class CreateSalesReturnItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal? TaxPercent { get; set; }
    public string? Reason { get; set; }
}

// ── Sales Debit Note ────────────────────────────────────────────────

public class SalesDebitNoteDto
{
    public int Id { get; set; }
    public string NoteNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class CreateSalesDebitNoteDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? SalesInvoiceId { get; set; }
    public decimal Amount { get; set; }
    public decimal? TaxAmount { get; set; }
    public string? Reason { get; set; }
}

// ── Sales Credit Note ───────────────────────────────────────────────

public class SalesCreditNoteDto
{
    public int Id { get; set; }
    public string NoteNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class CreateSalesCreditNoteDto
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int? SalesInvoiceId { get; set; }
    public int? SalesReturnId { get; set; }
    public decimal Amount { get; set; }
    public decimal? TaxAmount { get; set; }
    public string? Reason { get; set; }
}
