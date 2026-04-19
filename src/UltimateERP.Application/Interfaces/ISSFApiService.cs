namespace UltimateERP.Application.Interfaces;

public class SSFEmployeeDto
{
    public string EmployeeName { get; set; } = string.Empty;
    public string? SSFNumber { get; set; }
    public string? PanNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinDate { get; set; }
    public string? Gender { get; set; }
    public decimal BasicSalary { get; set; }
}

public class SSFContributionDto
{
    public string SSFNumber { get; set; } = string.Empty;
    public decimal EmployeeContribution { get; set; }
    public decimal EmployerContribution { get; set; }
    public string ContributionMonth { get; set; } = string.Empty;
    public int ContributionYear { get; set; }
}

public class SSFRegistrationResult
{
    public bool IsSuccess { get; set; }
    public string? SSFNumber { get; set; }
    public string? ErrorMessage { get; set; }
}

public class SSFContributionResult
{
    public bool IsSuccess { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface ISSFApiService
{
    Task<SSFRegistrationResult> RegisterEmployeeAsync(SSFEmployeeDto employee);
    Task<SSFContributionResult> SubmitContributionAsync(SSFContributionDto contribution);
}
