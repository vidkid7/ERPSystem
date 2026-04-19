using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Lab;

public class SampleCollection : BaseEntity
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
    public SampleCollectionStatus Status { get; set; }

    public ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();
}
