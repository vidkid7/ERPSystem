using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Support.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Support;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Support.Commands;

// Create Support Ticket
public record CreateSupportTicketCommand(CreateSupportTicketDto Ticket) : IRequest<ApiResponse<SupportTicketDto>>;

public class CreateSupportTicketValidator : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketValidator()
    {
        RuleFor(x => x.Ticket.TicketNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Ticket.Subject).NotEmpty().MaximumLength(200);
    }
}

public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, ApiResponse<SupportTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSupportTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SupportTicketDto>> Handle(CreateSupportTicketCommand request, CancellationToken ct)
    {
        var dto = request.Ticket;
        var exists = await _db.SupportTickets.AnyAsync(t => t.TicketNumber == dto.TicketNumber, ct);
        if (exists) return ApiResponse<SupportTicketDto>.Failure($"Ticket number {dto.TicketNumber} already exists");

        var priority = TicketPriority.Medium;
        if (!string.IsNullOrWhiteSpace(dto.Priority))
            Enum.TryParse(dto.Priority, out priority);

        var ticket = new SupportTicket
        {
            TicketNumber = dto.TicketNumber,
            TicketDate = dto.TicketDate,
            Subject = dto.Subject,
            Description = dto.Description,
            Priority = priority,
            Status = SupportTicketStatus.Open,
            CreatedById = dto.CreatedById
        };

        _db.SupportTickets.Add(ticket);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SupportTicketDto>.Success(_mapper.Map<SupportTicketDto>(ticket), "Support ticket created");
    }
}

// Assign Support Ticket
public record AssignSupportTicketCommand(AssignSupportTicketDto Assignment) : IRequest<ApiResponse<SupportTicketDto>>;

public class AssignSupportTicketHandler : IRequestHandler<AssignSupportTicketCommand, ApiResponse<SupportTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public AssignSupportTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SupportTicketDto>> Handle(AssignSupportTicketCommand request, CancellationToken ct)
    {
        var ticket = await _db.SupportTickets.FindAsync(new object[] { request.Assignment.TicketId }, ct);
        if (ticket is null) return ApiResponse<SupportTicketDto>.Failure("Support ticket not found");

        ticket.AssignedToId = request.Assignment.AssignedToId;
        ticket.Status = SupportTicketStatus.InProgress;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<SupportTicketDto>.Success(_mapper.Map<SupportTicketDto>(ticket), "Support ticket assigned");
    }
}

// Resolve Support Ticket
public record ResolveSupportTicketCommand(ResolveSupportTicketDto Dto) : IRequest<ApiResponse<SupportTicketDto>>;

public class ResolveSupportTicketHandler : IRequestHandler<ResolveSupportTicketCommand, ApiResponse<SupportTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ResolveSupportTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SupportTicketDto>> Handle(ResolveSupportTicketCommand request, CancellationToken ct)
    {
        var ticket = await _db.SupportTickets.FindAsync(new object[] { request.Dto.TicketId }, ct);
        if (ticket is null) return ApiResponse<SupportTicketDto>.Failure("Support ticket not found");

        ticket.Status = SupportTicketStatus.Resolved;
        ticket.ResolutionNotes = request.Dto.ResolutionNotes;
        ticket.ResolvedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<SupportTicketDto>.Success(_mapper.Map<SupportTicketDto>(ticket), "Support ticket resolved");
    }
}

// Escalate Support Ticket
public record EscalateSupportTicketCommand(EscalateSupportTicketDto Dto) : IRequest<ApiResponse<SupportTicketDto>>;

public class EscalateSupportTicketHandler : IRequestHandler<EscalateSupportTicketCommand, ApiResponse<SupportTicketDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public EscalateSupportTicketHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SupportTicketDto>> Handle(EscalateSupportTicketCommand request, CancellationToken ct)
    {
        var ticket = await _db.SupportTickets.FindAsync(new object[] { request.Dto.TicketId }, ct);
        if (ticket is null) return ApiResponse<SupportTicketDto>.Failure("Support ticket not found");

        ticket.AssignedToId = request.Dto.EscalatedToId;
        ticket.Status = SupportTicketStatus.InProgress;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<SupportTicketDto>.Success(_mapper.Map<SupportTicketDto>(ticket), "Support ticket escalated");
    }
}
