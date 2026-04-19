namespace UltimateERP.Application.Features.Industry.DTOs;

// ── Dairy Purchase ────────────────────────────────────────────────────

public class DairyPurchaseInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public decimal TotalQuantityLitre { get; set; }
    public decimal TotalFatKg { get; set; }
    public decimal TotalSNFKg { get; set; }
    public decimal NetAmount { get; set; }
    public bool IsPosted { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
}

public class CreateDairyPurchaseDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public int? RouteId { get; set; }
    public decimal TotalQuantityLitre { get; set; }
    public decimal TotalFatKg { get; set; }
    public decimal TotalSNFKg { get; set; }
    public decimal NetAmount { get; set; }
    public int BranchId { get; set; }
}

// ── Tea Purchase ──────────────────────────────────────────────────────

public class TeaPurchaseInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? GardenName { get; set; }
    public string? LotNumber { get; set; }
    public int? GradeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal NetAmount { get; set; }
    public bool IsPosted { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
}

public class CreateTeaPurchaseDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string? InvoiceDateBS { get; set; }
    public int VendorId { get; set; }
    public int? GodownId { get; set; }
    public int? CostClassId { get; set; }
    public string? GardenName { get; set; }
    public string? LotNumber { get; set; }
    public int? GradeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal NetAmount { get; set; }
    public int BranchId { get; set; }
}

// ── Petrol Pump ───────────────────────────────────────────────────────

public class PetrolPumpTransactionDto
{
    public int Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? TransactionDateBS { get; set; }
    public int? NozzleId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal OpeningReading { get; set; }
    public decimal ClosingReading { get; set; }
    public decimal QuantityDispensed { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public bool IsPosted { get; set; }
    public bool IsActive { get; set; }
    public int BranchId { get; set; }
}

public class CreatePetrolPumpTransactionDto
{
    public string TransactionNumber { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? TransactionDateBS { get; set; }
    public int? NozzleId { get; set; }
    public int ProductId { get; set; }
    public decimal OpeningReading { get; set; }
    public decimal ClosingReading { get; set; }
    public decimal QuantityDispensed { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public int? CustomerId { get; set; }
    public int? GodownId { get; set; }
    public int BranchId { get; set; }
}
