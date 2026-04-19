using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.HMS;

public class OPDTicket : BaseEntity
{
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int? DoctorId { get; set; }
    public int? OPDTicketTypeId { get; set; }
    public int? OPDServiceTypeId { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public string? Prescription { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
}
