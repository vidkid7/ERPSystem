using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Domain.Entities.HR;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? JoiningDate { get; set; }
    public int? DesignationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? PANNumber { get; set; }
}
