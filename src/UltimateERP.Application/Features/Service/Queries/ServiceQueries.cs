using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Service.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Service.Queries;

// Complaint Tickets
public record GetComplaintTicketsQuery(int? CustomerId, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ComplaintTicketDto>>>;

public class GetComplaintTicketsHandler : IRequestHandler<GetComplaintTicketsQuery, ApiResponse<List<ComplaintTicketDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetComplaintTicketsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ComplaintTicketDto>>> Handle(GetComplaintTicketsQuery request, CancellationToken ct)
    {
        var query = _db.ComplaintTickets.Include(t => t.Customer).AsQueryable();
        if (request.CustomerId.HasValue) query = query.Where(t => t.CustomerId == request.CustomerId);
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.ComplaintTicketStatus>(request.Status, out var status))
            query = query.Where(t => t.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.TicketDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<ComplaintTicketDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ComplaintTicketDto>>.Success(items, "Complaint tickets retrieved", total);
    }
}

// Job Cards
public record GetJobCardsQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<JobCardDto>>>;

public class GetJobCardsHandler : IRequestHandler<GetJobCardsQuery, ApiResponse<List<JobCardDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetJobCardsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<JobCardDto>>> Handle(GetJobCardsQuery request, CancellationToken ct)
    {
        var query = _db.JobCards
            .Include(j => j.ComplaintTicket).Include(j => j.AssignedTo).AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.JobCardStatus>(request.Status, out var status))
            query = query.Where(j => j.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(j => j.JobCardDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<JobCardDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<JobCardDto>>.Success(items, "Job cards retrieved", total);
    }
}

// Service Appointments
public record GetServiceAppointmentsQuery(int? CustomerId, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ServiceAppointmentDto>>>;

public class GetServiceAppointmentsHandler : IRequestHandler<GetServiceAppointmentsQuery, ApiResponse<List<ServiceAppointmentDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetServiceAppointmentsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ServiceAppointmentDto>>> Handle(GetServiceAppointmentsQuery request, CancellationToken ct)
    {
        var query = _db.ServiceAppointments.Include(a => a.Customer).Include(a => a.AssignedTo).AsQueryable();
        if (request.CustomerId.HasValue) query = query.Where(a => a.CustomerId == request.CustomerId);
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.ServiceAppointmentStatus>(request.Status, out var status))
            query = query.Where(a => a.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AppointmentDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<ServiceAppointmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ServiceAppointmentDto>>.Success(items, "Service appointments retrieved", total);
    }
}
