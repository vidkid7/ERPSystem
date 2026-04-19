using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Service.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Service;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Service.Commands;

// Create Complaint Ticket
public record CreateComplaintTicketCommand(CreateComplaintTicketDto Ticket) : IRequest<ApiResponse<ComplaintTicketDto>>;

public class CreateComplaintTicketHandler : IRequestHandler<CreateComplaintTicketCommand, ApiResponse<ComplaintTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateComplaintTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ComplaintTicketDto>> Handle(CreateComplaintTicketCommand request, CancellationToken ct)
    {
        var dto = request.Ticket;
        var priority = Enum.TryParse<TicketPriority>(dto.Priority, true, out var p) ? p : TicketPriority.Medium;

        var ticket = new ComplaintTicket
        {
            TicketNumber = $"CT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            TicketDate = DateTime.UtcNow,
            CustomerId = dto.CustomerId,
            DeviceId = dto.DeviceId,
            ComplaintDescription = dto.ComplaintDescription,
            Priority = priority,
            Status = ComplaintTicketStatus.Open
        };

        _db.ComplaintTickets.Add(ticket);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ComplaintTicketDto>.Success(_mapper.Map<ComplaintTicketDto>(ticket), "Complaint ticket created");
    }
}

// Create Job Card
public record CreateJobCardCommand(CreateJobCardDto JobCard) : IRequest<ApiResponse<JobCardDto>>;

public class CreateJobCardHandler : IRequestHandler<CreateJobCardCommand, ApiResponse<JobCardDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateJobCardHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<JobCardDto>> Handle(CreateJobCardCommand request, CancellationToken ct)
    {
        var dto = request.JobCard;
        var jobCard = new JobCard
        {
            JobCardNumber = $"JC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            JobCardDate = DateTime.UtcNow,
            ComplaintTicketId = dto.ComplaintTicketId,
            JobTypeId = dto.JobTypeId,
            AssignedToId = dto.AssignedToId,
            EstimatedCost = dto.EstimatedCost,
            Status = JobCardStatus.Pending
        };

        _db.JobCards.Add(jobCard);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<JobCardDto>.Success(_mapper.Map<JobCardDto>(jobCard), "Job card created");
    }
}

// Complete Job Card
public record CompleteJobCardCommand(int JobCardId, decimal ActualCost) : IRequest<ApiResponse<JobCardDto>>;

public class CompleteJobCardHandler : IRequestHandler<CompleteJobCardCommand, ApiResponse<JobCardDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CompleteJobCardHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<JobCardDto>> Handle(CompleteJobCardCommand request, CancellationToken ct)
    {
        var jc = await _db.JobCards.FindAsync(new object[] { request.JobCardId }, ct);
        if (jc is null) return ApiResponse<JobCardDto>.Failure("Job card not found");

        jc.Status = JobCardStatus.Completed;
        jc.ActualCost = request.ActualCost;
        jc.CompletionDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<JobCardDto>.Success(_mapper.Map<JobCardDto>(jc), "Job card completed");
    }
}

// Create Service Appointment
public record CreateServiceAppointmentCommand(CreateServiceAppointmentDto Appointment) : IRequest<ApiResponse<ServiceAppointmentDto>>;

public class CreateServiceAppointmentHandler : IRequestHandler<CreateServiceAppointmentCommand, ApiResponse<ServiceAppointmentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateServiceAppointmentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ServiceAppointmentDto>> Handle(CreateServiceAppointmentCommand request, CancellationToken ct)
    {
        var dto = request.Appointment;
        var appt = new ServiceAppointment
        {
            AppointmentNumber = $"SA-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            AppointmentDate = dto.AppointmentDate,
            CustomerId = dto.CustomerId,
            DeviceModelId = dto.DeviceModelId,
            ServiceTypeId = dto.ServiceTypeId,
            AssignedToId = dto.AssignedToId,
            Status = ServiceAppointmentStatus.Scheduled,
            Notes = dto.Notes
        };

        _db.ServiceAppointments.Add(appt);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ServiceAppointmentDto>.Success(_mapper.Map<ServiceAppointmentDto>(appt), "Service appointment created");
    }
}
