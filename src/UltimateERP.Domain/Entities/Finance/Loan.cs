using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Finance;

public class Loan : BaseEntity
{
    public string LoanNumber { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public string? BorrowerName { get; set; }
    public string? BorrowerContact { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public decimal EMIAmount { get; set; }
    public LoanStatus Status { get; set; }
    public int? VehicleDetailId { get; set; }
    public VehicleDetail? VehicleDetail { get; set; }

    public ICollection<LoanEMI> EMIs { get; set; } = new List<LoanEMI>();
}
