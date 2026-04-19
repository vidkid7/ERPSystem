namespace UltimateERP.Application.Features.Nepal.DTOs;

// IRD Sales Data DTO for CBMS API submission
public class IRDSalesDataDto
{
    public string InvoiceNo { get; set; } = string.Empty;
    public string InvoiceDate { get; set; } = string.Empty;
    public string? CustomerPAN { get; set; }
    public string? CustomerName { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ExciseAmount { get; set; }
    public string? FiscalYear { get; set; }
    public bool IsBillPrinted { get; set; }
    public bool IsBillActive { get; set; } = true;
}

// IRD Purchase Data DTO for CBMS API submission
public class IRDPurchaseDataDto
{
    public string InvoiceNo { get; set; } = string.Empty;
    public string InvoiceDate { get; set; } = string.Empty;
    public string? SupplierPAN { get; set; }
    public string? SupplierName { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ExciseAmount { get; set; }
    public string? FiscalYear { get; set; }
}

// IRD Submission Result
public class IRDSubmissionResultDto
{
    public bool IsSuccess { get; set; }
    public string? SubmissionId { get; set; }
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime SubmittedAt { get; set; }
}

// Annex 10 VAT Report Item
public class Annex10ItemDto
{
    public int SN { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public string InvoiceDate { get; set; } = string.Empty;
    public string? CustomerPAN { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public decimal TaxableSalesAmount { get; set; }
    public decimal VATAmount { get; set; }
    public decimal ExemptSalesAmount { get; set; }
    public decimal ExportSalesAmount { get; set; }
}

// Excise Register Item
public class ExciseRegisterItemDto
{
    public int SN { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public string InvoiceDate { get; set; } = string.Empty;
    public string? ProductName { get; set; }
    public string? HSCode { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal ExciseRate { get; set; }
    public decimal ExciseAmount { get; set; }
    public decimal TotalAmount { get; set; }
}

// One Lakh Above Sales Report Item (required by Nepal tax law for high-value transactions)
public class OneLakhAboveDto
{
    public int SN { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public string InvoiceDate { get; set; } = string.Empty;
    public string CustomerPAN { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal VATAmount { get; set; }
}
