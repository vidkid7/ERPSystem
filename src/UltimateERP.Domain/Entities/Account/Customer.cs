using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Account;

public class Customer : BaseEntity
{
    public int LedgerId { get; set; }
    public Ledger Ledger { get; set; } = null!;

    public int? DebtorTypeId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public int? DebtorRouteId { get; set; }
    public int? SalesAgentId { get; set; }
    public int? AreaId { get; set; }
    public int? ClusterId { get; set; }
    public int? CreditGroupId { get; set; }
    public decimal CreditLimit { get; set; }
    public int CreditDays { get; set; }
    public int? PricingGroupId { get; set; }
    public bool IsPending { get; set; }
}
