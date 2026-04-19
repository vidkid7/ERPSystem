namespace UltimateERP.Application.Features.Reporting.DTOs;

public class StockAgingDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public int DaysInStock { get; set; }
    public decimal Value { get; set; }
    public string AgingBucket { get; set; } = string.Empty; // 0-30, 31-60, 61-90, 90+
}

public class StockMovementDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal OpeningQty { get; set; }
    public decimal InwardQty { get; set; }
    public decimal OutwardQty { get; set; }
    public decimal ClosingQty { get; set; }
}

public class SalesAnalysisDto
{
    public string GroupName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}

public class PurchaseAnalysisDto
{
    public string VendorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}

public class AgentPerformanceDto
{
    public int AgentId { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public decimal TotalCollection { get; set; }
    public decimal OutstandingAmount { get; set; }
}
