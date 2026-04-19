namespace UltimateERP.Application.Features.Integration.DTOs;

// ── SMS DTOs ──────────────────────────────────────────────────────────

public class SendSmsDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SendBulkSmsDto
{
    public List<string> PhoneNumbers { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

public class SmsLogDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsSent { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentDate { get; set; }
}

// ── Payment DTOs ──────────────────────────────────────────────────────

public class PaymentRequestDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "NPR";
    public string? ReferenceId { get; set; }
    public string? CustomerName { get; set; }
    public string? ReturnUrl { get; set; }
}

public class PaymentResultDto
{
    public bool IsSuccess { get; set; }
    public string? TransactionId { get; set; }
    public string? RedirectUrl { get; set; }
    public string? ErrorMessage { get; set; }
}

public class PaymentVerificationDto
{
    public string TransactionId { get; set; } = string.Empty;
}

// ── SSF DTOs ──────────────────────────────────────────────────────────

public class SSFRegistrationRequestDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string? PanNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinDate { get; set; }
    public string? Gender { get; set; }
    public decimal BasicSalary { get; set; }
}

public class SSFContributionRequestDto
{
    public string SSFNumber { get; set; } = string.Empty;
    public decimal EmployeeContribution { get; set; }
    public decimal EmployerContribution { get; set; }
    public string ContributionMonth { get; set; } = string.Empty;
    public int ContributionYear { get; set; }
}

public class SSFRegistrationResultDto
{
    public bool IsSuccess { get; set; }
    public string? SSFNumber { get; set; }
    public string? ErrorMessage { get; set; }
}

public class SSFContributionResultDto
{
    public bool IsSuccess { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ErrorMessage { get; set; }
}

// ── Import/Export DTOs ────────────────────────────────────────────────

public class ImportResultDto
{
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class TallyExportRequestDto
{
    public string ExportType { get; set; } = string.Empty; // Ledgers, Vouchers, Products
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class TallyImportResultDto
{
    public int TotalRecords { get; set; }
    public int ImportedCount { get; set; }
    public int SkippedCount { get; set; }
    public List<string> Errors { get; set; } = new();
}
