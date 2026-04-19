using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Support.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Support.Queries;

public record GetSupportTicketsQuery(string? Status, string? Search, int? AssignedToId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SupportTicketDto>>>;

public class GetSupportTicketsHandler : IRequestHandler<GetSupportTicketsQuery, ApiResponse<List<SupportTicketDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSupportTicketsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SupportTicketDto>>> Handle(GetSupportTicketsQuery request, CancellationToken ct)
    {
        var query = _db.SupportTickets
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedByUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.SupportTicketStatus>(request.Status, out var status))
            query = query.Where(t => t.Status == status);
        if (request.AssignedToId.HasValue)
            query = query.Where(t => t.AssignedToId == request.AssignedToId);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(t => t.TicketNumber.ToLower().Contains(s)
                                  || (t.Subject != null && t.Subject.ToLower().Contains(s)));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.TicketDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<SupportTicketDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SupportTicketDto>>.Success(items, "Support tickets retrieved", total);
    }
}
