using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Purchase.DTOs;

// ── Purchase Quotation ───────────────────────────────────────────────

public class PurchaseQuotationDto
{
    public int Id { get; set; }
    public string QuotationNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }
    public List<PurchaseQuotationItemDto> Items { get; set; } = new();
}

public class PurchaseQuotationItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

public class CreatePurchaseQuotationDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string? Remarks { get; set; }
    public List<CreatePurchaseQuotationItemDto> Items { get; set; } = new();
}

public class CreatePurchaseQuotationItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
}

// ── Purchase Order ───────────────────────────────────────────────────

public class PurchaseOrderDto
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public int? QuotationRef { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Remarks { get; set; }
    public List<PurchaseOrderItemDto> Items { get; set; } = new();
}

public class PurchaseOrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Qty { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}

public class CreatePurchaseOrderDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public int? QuotationRef { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Remarks { get; set; }
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}

public class CreatePurchaseOrderItemDto
{
    public int ProductId { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
}

// ── Receipt Note (GRN) ──────────────────────────────────────────────

public class ReceiptNoteDto
{
    public int Id { get; set; }
    public string GRNNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public int? PurchaseOrderRef { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
    public List<ReceiptNoteItemDto> Items { get; set; } = new();
}

public class ReceiptNoteItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal AcceptedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public decimal Rate { get; set; }
}

public class CreateReceiptNoteDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public int? PurchaseOrderRef { get; set; }
    public int? GodownId { get; set; }
    public string? Remarks { get; set; }
    public List<CreateReceiptNoteItemDto> Items { get; set; } = new();
}

public class CreateReceiptNoteItemDto
{
    public int ProductId { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal AcceptedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public decimal Rate { get; set; }
}

// ── Purchase Return ──────────────────────────────────────────────────

public class PurchaseReturnDto
{
    public int Id { get; set; }
    public string ReturnNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? InvoiceRef { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
    public List<PurchaseReturnItemDto> Items { get; set; } = new();
}

public class PurchaseReturnItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal ReturnQty { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}

public class CreatePurchaseReturnDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? InvoiceRef { get; set; }
    public int? GodownId { get; set; }
    public string? Remarks { get; set; }
    public List<CreatePurchaseReturnItemDto> Items { get; set; } = new();
}

public class CreatePurchaseReturnItemDto
{
    public int ProductId { get; set; }
    public decimal ReturnQty { get; set; }
    public decimal Rate { get; set; }
    public string? Reason { get; set; }
}

// ── Purchase Debit Note ──────────────────────────────────────────────

public class PurchaseDebitNoteDto
{
    public int Id { get; set; }
    public string NoteNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class CreatePurchaseDebitNoteDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}

// ── Purchase Credit Note ─────────────────────────────────────────────

public class PurchaseCreditNoteDto
{
    public int Id { get; set; }
    public string NoteNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class CreatePurchaseCreditNoteDto
{
    public DateTime Date { get; set; }
    public int VendorId { get; set; }
    public string? InvoiceRef { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
}
