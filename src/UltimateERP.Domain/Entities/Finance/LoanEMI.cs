using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Finance;

public class LoanEMI : BaseEntity
{
    public int LoanId { get; set; }
    public Loan Loan { get; set; } = null!;
    public int EMINumber { get; set; }
    public DateTime EMIDueDate { get; set; }
    public decimal EMIAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? PaidDate { get; set; }
    public EMIStatus Status { get; set; }
}
