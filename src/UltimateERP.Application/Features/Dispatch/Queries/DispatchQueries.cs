using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Dispatch.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Dispatch.Queries;

// ── Get Dispatches ────────────────────────────────────────────────────

public record GetDispatchesQuery(string? Status, string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<DispatchOrderDto>>>;

public class GetDispatchesHandler : IRequestHandler<GetDispatchesQuery, ApiResponse<List<DispatchOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetDispatchesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<DispatchOrderDto>>> Handle(GetDispatchesQuery request, CancellationToken ct)
    {
        var query = _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .Where(d => d.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<DispatchStatus>(request.Status, true, out var status))
            query = query.Where(d => d.Status == status);

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(d => d.DispatchNumber.Contains(request.Search)
                || (d.Customer != null && d.Customer.Name != null && d.Customer.Name.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(d => d.DispatchDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DispatchOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<DispatchOrderDto>>.Success(items, "Dispatches retrieved", total);
    }
}

// ── Get Pending Dispatches ────────────────────────────────────────────

public record GetPendingDispatchesQuery(int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<DispatchOrderDto>>>;

public class GetPendingDispatchesHandler : IRequestHandler<GetPendingDispatchesQuery, ApiResponse<List<DispatchOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPendingDispatchesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<DispatchOrderDto>>> Handle(GetPendingDispatchesQuery request, CancellationToken ct)
    {
        var query = _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .Where(d => d.IsActive && d.Status == DispatchStatus.Pending);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(d => d.DispatchDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DispatchOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<DispatchOrderDto>>.Success(items, "Pending dispatches retrieved", total);
    }
}
