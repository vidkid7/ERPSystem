namespace UltimateERP.Application.Features.Inventory.DTOs;

// ── Rack ────────────────────────────────────────────────────────────

public class RackDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int GodownId { get; set; }
    public string? GodownName { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}

public class CreateRackDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int GodownId { get; set; }
    public string? Location { get; set; }
}

// ── Indent ──────────────────────────────────────────────────────────

public class IndentDto
{
    public int Id { get; set; }
    public string IndentNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int? RequestedByEmployeeId { get; set; }
    public string? RequestedBy { get; set; }
    public List<IndentItemDto> Items { get; set; } = new();
    public string? Status { get; set; }
    public string? Remarks { get; set; }
}

public class IndentItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal RequestedQty { get; set; }
    public decimal ApprovedQty { get; set; }
    public string? Remarks { get; set; }
}

public class CreateIndentDto
{
    public DateTime Date { get; set; }
    public int? RequestedByEmployeeId { get; set; }
    public int? GodownId { get; set; }
    public string? Remarks { get; set; }
    public List<CreateIndentItemDto> Items { get; set; } = new();
}

public class CreateIndentItemDto
{
    public int ProductId { get; set; }
    public decimal RequestedQty { get; set; }
    public string? Remarks { get; set; }
}

// ── GatePass ────────────────────────────────────────────────────────

public class GatePassDto
{
    public int Id { get; set; }
    public string GatePassNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Type { get; set; }
    public string? VehicleNo { get; set; }
    public string? PersonName { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
    public bool IsApproved { get; set; }
}

public class CreateGatePassDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = "Inward";
    public string? VehicleNo { get; set; }
    public string? PersonName { get; set; }
    public string? Purpose { get; set; }
    public string? Description { get; set; }
    public int? GodownId { get; set; }
}

// ── StockDemand ─────────────────────────────────────────────────────

public class StockDemandDto
{
    public int Id { get; set; }
    public string DemandNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int? GodownId { get; set; }
    public string? GodownName { get; set; }
    public string? Status { get; set; }
}

public class CreateStockDemandDto
{
    public DateTime Date { get; set; }
    public int? GodownId { get; set; }
    public int? JobCardId { get; set; }
    public int? CostClassId { get; set; }
}

// ── Landed Cost ─────────────────────────────────────────────────────

public class LandedCostDto
{
    public int Id { get; set; }
    public int PurchaseInvoiceId { get; set; }
    public string? PurchaseInvoiceNumber { get; set; }
    public string CostType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? AllocationMethod { get; set; }
    public string? Description { get; set; }
}

public class CreateLandedCostDto
{
    public int PurchaseInvoiceId { get; set; }
    public string CostType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string AllocationMethod { get; set; } = "ByValue";
    public string? Description { get; set; }
}
