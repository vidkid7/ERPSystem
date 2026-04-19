namespace UltimateERP.Application.Features.Support.DTOs;

public class SupportTicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public int? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSupportTicketDto
{
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public int? CreatedById { get; set; }
}

public class AssignSupportTicketDto
{
    public int TicketId { get; set; }
    public int AssignedToId { get; set; }
}

public class ResolveSupportTicketDto
{
    public int TicketId { get; set; }
    public string? ResolutionNotes { get; set; }
}

public class EscalateSupportTicketDto
{
    public int TicketId { get; set; }
    public int EscalatedToId { get; set; }
    public string? Reason { get; set; }
}
