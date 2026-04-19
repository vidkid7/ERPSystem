namespace UltimateERP.Application.Features.Account.DTOs;

public class FiscalYearDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? StartDateBS { get; set; }
    public string? EndDateBS { get; set; }
    public string? Status { get; set; }
    public bool IsCurrent { get; set; }
}

public class ClosingTrialBalanceDto
{
    public int LedgerId { get; set; }
    public string? LedgerCode { get; set; }
    public string? LedgerName { get; set; }
    public string? GroupName { get; set; }
    public decimal OpeningDebit { get; set; }
    public decimal OpeningCredit { get; set; }
    public decimal TransactionDebit { get; set; }
    public decimal TransactionCredit { get; set; }
    public decimal ClosingDebit { get; set; }
    public decimal ClosingCredit { get; set; }
}

public class YearEndClosingResultDto
{
    public bool IsSuccess { get; set; }
    public int FiscalYearId { get; set; }
    public int OpeningVouchersCreated { get; set; }
    public string? Message { get; set; }
}
