using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.HMS;

public class IPDAdmission : BaseEntity
{
    public string AdmissionNumber { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int? DoctorId { get; set; }
    public int? AdmissionTypeId { get; set; }
    public int? BedId { get; set; }
    public Bed? Bed { get; set; }
    public string? Diagnosis { get; set; }
    public IPDStatus Status { get; set; }
    public DateTime? DischargeDate { get; set; }
    public int? DischargeTypeId { get; set; }
}
