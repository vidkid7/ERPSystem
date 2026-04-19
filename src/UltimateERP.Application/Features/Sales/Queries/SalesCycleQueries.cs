using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Sales.Queries;

// ── Sales Quotation Queries ─────────────────────────────────────────

public record GetSalesQuotationsQuery(
    int? CustomerId, DateTime? FromDate, DateTime? ToDate, string? Status,
    int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<SalesQuotationDto>>>;

public class GetSalesQuotationsHandler : IRequestHandler<GetSalesQuotationsQuery, ApiResponse<List<SalesQuotationDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSalesQuotationsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesQuotationDto>>> Handle(GetSalesQuotationsQuery request, CancellationToken ct)
    {
        var query = _db.SalesQuotations
            .Include(q => q.Customer)
            .Include(q => q.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(q => q.CustomerId == request.CustomerId);
        if (request.FromDate.HasValue) query = query.Where(q => q.QuotationDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(q => q.QuotationDate <= request.ToDate);
        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<SalesQuotationStatus>(request.Status, true, out var status))
            query = query.Where(q => q.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(q => q.QuotationDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SalesQuotationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesQuotationDto>>.Success(items, "Sales quotations retrieved", total);
    }
}

// ── Sales Order Queries ─────────────────────────────────────────────

public record GetSalesOrdersQuery(
    int? CustomerId, DateTime? FromDate, DateTime? ToDate, string? Status,
    int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<SalesOrderDto>>>;

public class GetSalesOrdersHandler : IRequestHandler<GetSalesOrdersQuery, ApiResponse<List<SalesOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSalesOrdersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesOrderDto>>> Handle(GetSalesOrdersQuery request, CancellationToken ct)
    {
        var query = _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(o => o.CustomerId == request.CustomerId);
        if (request.FromDate.HasValue) query = query.Where(o => o.OrderDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(o => o.OrderDate <= request.ToDate);
        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<SalesOrderStatus>(request.Status, true, out var status))
            query = query.Where(o => o.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SalesOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesOrderDto>>.Success(items, "Sales orders retrieved", total);
    }
}

// ── Pending Sales Orders (not fully delivered) ──────────────────────

public record GetPendingSalesOrdersQuery(int? CustomerId)
    : IRequest<ApiResponse<List<SalesOrderDto>>>;

public class GetPendingSalesOrdersHandler : IRequestHandler<GetPendingSalesOrdersQuery, ApiResponse<List<SalesOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetPendingSalesOrdersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesOrderDto>>> Handle(GetPendingSalesOrdersQuery request, CancellationToken ct)
    {
        var query = _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .Where(o => o.Status == SalesOrderStatus.Approved || o.Status == SalesOrderStatus.PartiallyDelivered)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(o => o.CustomerId == request.CustomerId);

        var items = await query
            .OrderBy(o => o.ExpectedDeliveryDate ?? o.OrderDate)
            .ProjectTo<SalesOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesOrderDto>>.Success(items, "Pending sales orders retrieved", items.Count);
    }
}

// ── Sales Allotment Queries ─────────────────────────────────────────

public record GetSalesAllotmentsQuery(int? CustomerId, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SalesAllotmentDto>>>;

public class GetSalesAllotmentsHandler : IRequestHandler<GetSalesAllotmentsQuery, ApiResponse<List<SalesAllotmentDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSalesAllotmentsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesAllotmentDto>>> Handle(GetSalesAllotmentsQuery request, CancellationToken ct)
    {
        var query = _db.SalesAllotments
            .Include(a => a.Customer)
            .Include(a => a.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(a => a.CustomerId == request.CustomerId);
        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<SalesAllotmentStatus>(request.Status, true, out var status))
            query = query.Where(a => a.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AllotmentDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SalesAllotmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesAllotmentDto>>.Success(items, "Sales allotments retrieved", total);
    }
}

// ── Pending Deliveries ──────────────────────────────────────────────

public record GetPendingDeliveriesQuery(int? CustomerId)
    : IRequest<ApiResponse<List<SalesDeliveryNoteDto>>>;

public class GetPendingDeliveriesHandler : IRequestHandler<GetPendingDeliveriesQuery, ApiResponse<List<SalesDeliveryNoteDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetPendingDeliveriesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesDeliveryNoteDto>>> Handle(GetPendingDeliveriesQuery request, CancellationToken ct)
    {
        var query = _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .Where(d => d.Status == DispatchStatus.Pending)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(d => d.CustomerId == request.CustomerId);

        var items = await query
            .OrderBy(d => d.DispatchDate)
            .ProjectTo<SalesDeliveryNoteDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesDeliveryNoteDto>>.Success(items, "Pending deliveries retrieved", items.Count);
    }
}

// ── Sales Return Queries ────────────────────────────────────────────

public record GetSalesReturnsQuery(int? CustomerId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SalesReturnDto>>>;

public class GetSalesReturnsHandler : IRequestHandler<GetSalesReturnsQuery, ApiResponse<List<SalesReturnDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSalesReturnsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesReturnDto>>> Handle(GetSalesReturnsQuery request, CancellationToken ct)
    {
        var query = _db.SalesReturns
            .Include(r => r.Customer)
            .Include(r => r.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(r => r.CustomerId == request.CustomerId);
        if (request.FromDate.HasValue) query = query.Where(r => r.ReturnDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(r => r.ReturnDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReturnDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SalesReturnDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesReturnDto>>.Success(items, "Sales returns retrieved", total);
    }
}
