using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.HMS;

public class Patient : BaseEntity
{
    public string PatientNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? EthnicityId { get; set; }
    public int? DisabilityId { get; set; }
    public DateTime RegistrationDate { get; set; }

    public ICollection<OPDTicket> OPDTickets { get; set; } = new List<OPDTicket>();
    public ICollection<IPDAdmission> IPDAdmissions { get; set; } = new List<IPDAdmission>();
}
