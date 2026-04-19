namespace UltimateERP.Application.Features.Finance.DTOs;

public class LoanDto
{
    public int Id { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public string? BorrowerName { get; set; }
    public string? BorrowerContact { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public decimal EMIAmount { get; set; }
    public string? Status { get; set; }
    public int? VehicleDetailId { get; set; }
    public bool IsActive { get; set; }
}

public class CreateLoanDto
{
    public string LoanNumber { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public string? BorrowerName { get; set; }
    public string? BorrowerContact { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public int? VehicleDetailId { get; set; }
}

public class LoanEMIDto
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public string? LoanNumber { get; set; }
    public int EMINumber { get; set; }
    public DateTime EMIDueDate { get; set; }
    public decimal EMIAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Status { get; set; }
}

public class ProcessEMIDto
{
    public int LoanEMIId { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaidDate { get; set; }
}
