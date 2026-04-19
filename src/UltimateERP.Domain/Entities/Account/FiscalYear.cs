using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Account;

public class FiscalYear : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? StartDateBS { get; set; }
    public string? EndDateBS { get; set; }
    public FiscalYearStatus Status { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime? ClosedDate { get; set; }
    public int? ClosedBy { get; set; }
}
