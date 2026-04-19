using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Service;

public class ComplaintTicket : BaseEntity
{
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? DeviceId { get; set; }
    public string? ComplaintDescription { get; set; }
    public int? TicketForId { get; set; }
    public int? NatureId { get; set; }
    public int? SourceId { get; set; }
    public TicketPriority Priority { get; set; }
    public ComplaintTicketStatus Status { get; set; }

    public ICollection<JobCard> JobCards { get; set; } = new List<JobCard>();
}
