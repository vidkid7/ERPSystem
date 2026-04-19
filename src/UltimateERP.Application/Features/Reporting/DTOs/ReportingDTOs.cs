namespace UltimateERP.Application.Features.Reporting.DTOs;

public class LedgerBalanceDto
{
    public int LedgerId { get; set; }
    public string? LedgerName { get; set; }
    public string? LedgerCode { get; set; }
    public string? GroupName { get; set; }
    public decimal DebitTotal { get; set; }
    public decimal CreditTotal { get; set; }
    public decimal Balance { get; set; }
    public string BalanceType => Balance >= 0 ? "Dr" : "Cr";
}

public class TrialBalanceDto
{
    public DateTime AsOfDate { get; set; }
    public List<LedgerBalanceDto> Ledgers { get; set; } = new();
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsBalanced => Math.Abs(TotalDebit - TotalCredit) < 0.01m;
}

public class DayBookEntryDto
{
    public int VoucherId { get; set; }
    public string VoucherNumber { get; set; } = string.Empty;
    public DateTime VoucherDate { get; set; }
    public string? VoucherType { get; set; }
    public string? Narration { get; set; }
    public List<DayBookDetailDto> Details { get; set; } = new();
}

public class DayBookDetailDto
{
    public string? LedgerName { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
}

public class ProfitLossDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetProfitLoss { get; set; }
    public List<LedgerBalanceDto> IncomeItems { get; set; } = new();
    public List<LedgerBalanceDto> ExpenseItems { get; set; } = new();
}

public class BalanceSheetDto
{
    public DateTime AsOfDate { get; set; }
    public List<LedgerBalanceDto> Assets { get; set; } = new();
    public List<LedgerBalanceDto> Liabilities { get; set; } = new();
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }
    public decimal NetWorth { get; set; }
}
