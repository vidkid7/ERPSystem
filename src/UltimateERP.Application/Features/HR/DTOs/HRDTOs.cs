namespace UltimateERP.Application.Features.HR.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? JoiningDate { get; set; }
    public int? DesignationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
}

public class CreateEmployeeDto
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
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? JoiningDate { get; set; }
    public int? DesignationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? BranchId { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? PANNumber { get; set; }
}

public class AttendanceDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime AttendanceDate { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public decimal WorkingHours { get; set; }
    public string? Status { get; set; }
}

public class CreateAttendanceDto
{
    public int EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public string? Remarks { get; set; }
}

public class LeaveDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalDays { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class CreateLeaveDto
{
    public int EmployeeId { get; set; }
    public int? LeaveTypeId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string? Reason { get; set; }
}

public class ExpenseClaimDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime ClaimDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
}

public class CreateExpenseClaimDto
{
    public int EmployeeId { get; set; }
    public DateTime ClaimDate { get; set; }
    public decimal TotalAmount { get; set; }
}
