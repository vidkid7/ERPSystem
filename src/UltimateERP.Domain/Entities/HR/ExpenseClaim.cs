using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.HR;

public class ExpenseClaim : BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime ClaimDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ExpenseClaimStatus Status { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? PaidDate { get; set; }
}
