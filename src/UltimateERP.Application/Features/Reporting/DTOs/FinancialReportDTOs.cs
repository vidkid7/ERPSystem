namespace UltimateERP.Application.Features.Reporting.DTOs;

// Section item used by balance sheet and P&L grouped reports
public class ReportSectionItemDto
{
    public string LedgerGroupName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class ReportSectionDto
{
    public string SectionName { get; set; } = string.Empty;
    public List<ReportSectionItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
}

// Enhanced Balance Sheet with sections
public class BalanceSheetReportDto
{
    public DateTime AsOfDate { get; set; }
    public ReportSectionDto Assets { get; set; } = new() { SectionName = "Assets" };
    public ReportSectionDto Liabilities { get; set; } = new() { SectionName = "Liabilities" };
    public decimal NetWorth { get; set; }
}

// Enhanced Profit & Loss with sections
public class ProfitLossReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public ReportSectionDto Income { get; set; } = new() { SectionName = "Income" };
    public ReportSectionDto Expense { get; set; } = new() { SectionName = "Expense" };
    public decimal NetProfit { get; set; }
}

// Cash Flow Statement
public class CashFlowDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal OperatingActivities { get; set; }
    public decimal InvestingActivities { get; set; }
    public decimal FinancingActivities { get; set; }
    public decimal NetCashFlow { get; set; }
}

// VAT Report
public class VATReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal SalesVAT { get; set; }
    public decimal PurchaseVAT { get; set; }
    public decimal VATPayable { get; set; }
}

// TDS Report item
public class TDSReportItemDto
{
    public string PartyName { get; set; } = string.Empty;
    public string? PanNo { get; set; }
    public decimal Amount { get; set; }
    public decimal TDSRate { get; set; }
    public decimal TDSAmount { get; set; }
}

public class TDSReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<TDSReportItemDto> Items { get; set; } = new();
    public decimal TotalTDSAmount { get; set; }
}
