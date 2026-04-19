namespace UltimateERP.Application.Features.Loyalty.DTOs;

public class MembershipPointDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? TransactionType { get; set; }
    public decimal Points { get; set; }
    public int? SalesInvoiceId { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
}

public class AccruePointsDto
{
    public int CustomerId { get; set; }
    public decimal Points { get; set; }
    public int? SalesInvoiceId { get; set; }
}

public class RedeemPointsDto
{
    public int CustomerId { get; set; }
    public decimal Points { get; set; }
}

public class PointsBalanceDto
{
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal CurrentBalance { get; set; }
}
