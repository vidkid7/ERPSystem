using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Support;

public class SupportTicket : BaseEntity
{
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public TicketPriority Priority { get; set; }
    public SupportTicketStatus Status { get; set; }
    public int? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }
    public int? CreatedById { get; set; }
    public User? CreatedByUser { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedDate { get; set; }
}
