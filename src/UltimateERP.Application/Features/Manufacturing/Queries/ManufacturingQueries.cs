using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Manufacturing.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Manufacturing.Queries;

// Get BOMs
public record GetBOMsQuery(int? ProductId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<BOMDto>>>;

public class GetBOMsHandler : IRequestHandler<GetBOMsQuery, ApiResponse<List<BOMDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetBOMsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BOMDto>>> Handle(GetBOMsQuery request, CancellationToken ct)
    {
        var query = _db.BOMs
            .Include(b => b.Product)
            .Include(b => b.Details).ThenInclude(d => d.ComponentProduct)
            .AsQueryable();

        if (request.ProductId.HasValue)
            query = query.Where(b => b.ProductId == request.ProductId.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(b => b.BOMDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<BOMDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BOMDto>>.Success(items, "BOMs retrieved", total);
    }
}

// Get Production Orders
public record GetProductionOrdersQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ProductionOrderDto>>>;

public class GetProductionOrdersHandler : IRequestHandler<GetProductionOrdersQuery, ApiResponse<List<ProductionOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetProductionOrdersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ProductionOrderDto>>> Handle(GetProductionOrdersQuery request, CancellationToken ct)
    {
        var query = _db.ProductionOrders
            .Include(p => p.FinishedProduct)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<ProductionOrderStatus>(request.Status, true, out var status))
            query = query.Where(p => p.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.OrderDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<ProductionOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ProductionOrderDto>>.Success(items, "Production orders retrieved", total);
    }
}
