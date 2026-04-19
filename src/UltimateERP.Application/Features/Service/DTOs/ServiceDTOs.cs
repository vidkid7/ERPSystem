namespace UltimateERP.Application.Features.Service.DTOs;

public class ComplaintTicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime TicketDate { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? ComplaintDescription { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
}

public class CreateComplaintTicketDto
{
    public int? CustomerId { get; set; }
    public int? DeviceId { get; set; }
    public string? ComplaintDescription { get; set; }
    public string? Priority { get; set; }
}

public class JobCardDto
{
    public int Id { get; set; }
    public string JobCardNumber { get; set; } = string.Empty;
    public DateTime JobCardDate { get; set; }
    public int? ComplaintTicketId { get; set; }
    public string? ComplaintTicketNumber { get; set; }
    public string? AssignedToName { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }
    public string? Status { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class CreateJobCardDto
{
    public int? ComplaintTicketId { get; set; }
    public int? JobTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public decimal EstimatedCost { get; set; }
}

public class ServiceAppointmentDto
{
    public int Id { get; set; }
    public string AppointmentNumber { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? AssignedToName { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class CreateServiceAppointmentDto
{
    public int? CustomerId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public int? DeviceModelId { get; set; }
    public int? ServiceTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public string? Notes { get; set; }
}
