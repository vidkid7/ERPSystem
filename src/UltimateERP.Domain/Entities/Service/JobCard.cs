using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Service;

public class JobCard : BaseEntity
{
    public string JobCardNumber { get; set; } = string.Empty;
    public DateTime JobCardDate { get; set; }
    public int? ComplaintTicketId { get; set; }
    public ComplaintTicket? ComplaintTicket { get; set; }
    public int? JobTypeId { get; set; }
    public int? JobCardTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public Employee? AssignedTo { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }
    public JobCardStatus Status { get; set; }
    public DateTime? CompletionDate { get; set; }

    public ICollection<SparePartsDemand> SparePartsDemands { get; set; } = new List<SparePartsDemand>();
}
