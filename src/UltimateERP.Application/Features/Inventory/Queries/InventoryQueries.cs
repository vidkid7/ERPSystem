using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Inventory.Queries;

// ── Product Queries ───────────────────────────────────────────────────

public record GetProductsQuery(string? Search, int? GroupId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<ProductDto>>>;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, ApiResponse<List<ProductDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetProductsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var query = _db.Products.Include(p => p.ProductGroup).Where(p => p.IsActive).AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(p => p.Name.Contains(request.Search) || p.Code.Contains(request.Search));
        if (request.GroupId.HasValue)
            query = query.Where(p => p.ProductGroupId == request.GroupId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.Code)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ProductDto>>.Success(items, "Products retrieved", total);
    }
}

// ── ProductGroup Queries ──────────────────────────────────────────────

public record GetProductGroupTreeQuery : IRequest<ApiResponse<List<ProductGroupDto>>>;

public class GetProductGroupTreeHandler : IRequestHandler<GetProductGroupTreeQuery, ApiResponse<List<ProductGroupDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetProductGroupTreeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ProductGroupDto>>> Handle(GetProductGroupTreeQuery request, CancellationToken ct)
    {
        var allGroups = await _db.ProductGroups
            .Where(g => g.IsActive)
            .ProjectTo<ProductGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        var lookup = allGroups.ToLookup(g => g.ParentGroupId);
        var roots = allGroups.Where(g => g.ParentGroupId == null).ToList();

        void BuildTree(ProductGroupDto node)
        {
            node.Children = lookup[node.Id].ToList();
            foreach (var child in node.Children) BuildTree(child);
        }
        foreach (var root in roots) BuildTree(root);

        return ApiResponse<List<ProductGroupDto>>.Success(roots, "Product groups retrieved", allGroups.Count);
    }
}

// ── Godown Queries ────────────────────────────────────────────────────

public record GetGodownsQuery : IRequest<ApiResponse<List<GodownDto>>>;

public class GetGodownsHandler : IRequestHandler<GetGodownsQuery, ApiResponse<List<GodownDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetGodownsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<GodownDto>>> Handle(GetGodownsQuery request, CancellationToken ct)
    {
        var items = await _db.Godowns
            .Where(g => g.IsActive)
            .OrderBy(g => g.Code)
            .ProjectTo<GodownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<GodownDto>>.Success(items, "Godowns retrieved", items.Count);
    }
}

// ── Stock Queries ─────────────────────────────────────────────────────

public record GetStockQuery(int? ProductId, int? GodownId) : IRequest<ApiResponse<List<StockDto>>>;

public class GetStockHandler : IRequestHandler<GetStockQuery, ApiResponse<List<StockDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetStockHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<StockDto>>> Handle(GetStockQuery request, CancellationToken ct)
    {
        var query = _db.Stocks.Include(s => s.Product).Include(s => s.Godown).AsQueryable();

        if (request.ProductId.HasValue)
            query = query.Where(s => s.ProductId == request.ProductId);
        if (request.GodownId.HasValue)
            query = query.Where(s => s.GodownId == request.GodownId);

        var items = await query
            .ProjectTo<StockDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<StockDto>>.Success(items, "Stock retrieved", items.Count);
    }
}
