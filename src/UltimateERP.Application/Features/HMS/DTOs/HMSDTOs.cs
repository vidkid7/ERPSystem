using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.HMS.DTOs;

public class PatientDto
{
    public int Id { get; set; }
    public string PatientNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class CreatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public class BedDto
{
    public int Id { get; set; }
    public string BedNumber { get; set; } = string.Empty;
    public string? BedType { get; set; }
    public string? Status { get; set; }
}

public class OPDTicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Prescription { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
}

public class CreateOPDTicketDto
{
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string? Symptoms { get; set; }
    public decimal Amount { get; set; }
}

public class IPDAdmissionDto
{
    public int Id { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public int? BedId { get; set; }
    public string? BedNumber { get; set; }
    public string? Diagnosis { get; set; }
    public string? Status { get; set; }
    public DateTime? DischargeDate { get; set; }
}

public class CreateIPDAdmissionDto
{
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public int? BedId { get; set; }
    public string? Diagnosis { get; set; }
}
