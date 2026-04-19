using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Inventory.Queries;

// ── Rack Queries ────────────────────────────────────────────────────

public record GetRacksQuery(int? GodownId) : IRequest<ApiResponse<List<RackDto>>>;

public class GetRacksHandler : IRequestHandler<GetRacksQuery, ApiResponse<List<RackDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetRacksHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<RackDto>>> Handle(GetRacksQuery request, CancellationToken ct)
    {
        var query = _db.Racks.Include(r => r.Godown).Where(r => r.IsActive).AsQueryable();
        if (request.GodownId.HasValue) query = query.Where(r => r.GodownId == request.GodownId);

        var items = await query.OrderBy(r => r.Code)
            .ProjectTo<RackDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<RackDto>>.Success(items, "Racks retrieved", items.Count);
    }
}

// ── Indent Queries ──────────────────────────────────────────────────

public record GetIndentsQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<IndentDto>>>;

public class GetIndentsHandler : IRequestHandler<GetIndentsQuery, ApiResponse<List<IndentDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetIndentsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<IndentDto>>> Handle(GetIndentsQuery request, CancellationToken ct)
    {
        var query = _db.Indents
            .Include(i => i.RequestedByEmployee)
            .Include(i => i.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<IndentStatus>(request.Status, true, out var status))
            query = query.Where(i => i.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(i => i.IndentDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<IndentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<IndentDto>>.Success(items, "Indents retrieved", total);
    }
}

// ── GatePass Queries ────────────────────────────────────────────────

public record GetGatePassesQuery(string? Type, bool? IsApproved, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<GatePassDto>>>;

public class GetGatePassesHandler : IRequestHandler<GetGatePassesQuery, ApiResponse<List<GatePassDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetGatePassesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<GatePassDto>>> Handle(GetGatePassesQuery request, CancellationToken ct)
    {
        var query = _db.GatePasses.AsQueryable();

        if (!string.IsNullOrEmpty(request.Type) && Enum.TryParse<GatePassType>(request.Type, true, out var gpType))
            query = query.Where(g => g.GatePassType == gpType);
        if (request.IsApproved.HasValue)
            query = query.Where(g => g.IsApproved == request.IsApproved);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(g => g.GatePassDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<GatePassDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<GatePassDto>>.Success(items, "Gate passes retrieved", total);
    }
}

// ── StockDemand Queries ─────────────────────────────────────────────

public record GetStockDemandsQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<StockDemandDto>>>;

public class GetStockDemandsHandler : IRequestHandler<GetStockDemandsQuery, ApiResponse<List<StockDemandDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetStockDemandsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<StockDemandDto>>> Handle(GetStockDemandsQuery request, CancellationToken ct)
    {
        var query = _db.StockDemands.Include(d => d.Godown).AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<StockDemandStatus>(request.Status, true, out var status))
            query = query.Where(d => d.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(d => d.DemandDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<StockDemandDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<StockDemandDto>>.Success(items, "Stock demands retrieved", total);
    }
}

// ── Landed Cost Queries ─────────────────────────────────────────────

public record GetLandedCostsQuery(int? PurchaseInvoiceId) : IRequest<ApiResponse<List<LandedCostDto>>>;

public class GetLandedCostsHandler : IRequestHandler<GetLandedCostsQuery, ApiResponse<List<LandedCostDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetLandedCostsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LandedCostDto>>> Handle(GetLandedCostsQuery request, CancellationToken ct)
    {
        var query = _db.LandedCosts.Include(l => l.PurchaseInvoice).AsQueryable();
        if (request.PurchaseInvoiceId.HasValue)
            query = query.Where(l => l.PurchaseInvoiceId == request.PurchaseInvoiceId);

        var items = await query.OrderByDescending(l => l.CreatedDate)
            .ProjectTo<LandedCostDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LandedCostDto>>.Success(items, "Landed costs retrieved", items.Count);
    }
}
