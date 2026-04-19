namespace UltimateERP.Application.Features.Lab.DTOs;

public class SampleCollectionDto
{
    public int Id { get; set; }
    public string SampleNumber { get; set; } = string.Empty;
    public DateTime CollectionDate { get; set; }
    public string? CollectionDateBS { get; set; }
    public string? PatientName { get; set; }
    public int? PatientAge { get; set; }
    public string? PatientGender { get; set; }
    public string? PatientContact { get; set; }
    public string? DoctorName { get; set; }
    public string? TestParameters { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSampleCollectionDto
{
    public string SampleNumber { get; set; } = string.Empty;
    public DateTime CollectionDate { get; set; }
    public string? CollectionDateBS { get; set; }
    public string? PatientName { get; set; }
    public int? PatientAge { get; set; }
    public string? PatientGender { get; set; }
    public string? PatientContact { get; set; }
    public string? DoctorName { get; set; }
    public string? TestParameters { get; set; }
}

public class LabReportDto
{
    public int Id { get; set; }
    public int SampleCollectionId { get; set; }
    public string? SampleNumber { get; set; }
    public string? PatientName { get; set; }
    public DateTime ReportDate { get; set; }
    public string? ReportDateBS { get; set; }
    public int? TemplateId { get; set; }
    public string? ReportData { get; set; }
    public int? GeneratedBy { get; set; }
    public bool IsActive { get; set; }
}

public class CreateLabReportDto
{
    public int SampleCollectionId { get; set; }
    public DateTime ReportDate { get; set; }
    public string? ReportDateBS { get; set; }
    public int? TemplateId { get; set; }
    public string? ReportData { get; set; }
    public int? GeneratedBy { get; set; }
}
