using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Purchase.Queries;

// ── Purchase Quotation Queries ───────────────────────────────────────

public record GetPurchaseQuotationsQuery(int? VendorId, DateTime? FromDate, DateTime? ToDate, PurchaseDocumentStatus? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PurchaseQuotationDto>>>;

public class GetPurchaseQuotationsHandler : IRequestHandler<GetPurchaseQuotationsQuery, ApiResponse<List<PurchaseQuotationDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPurchaseQuotationsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PurchaseQuotationDto>>> Handle(GetPurchaseQuotationsQuery request, CancellationToken ct)
    {
        var query = _db.PurchaseQuotations
            .Include(q => q.Vendor).Include(q => q.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(q => q.VendorId == request.VendorId);
        if (request.FromDate.HasValue) query = query.Where(q => q.QuotationDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(q => q.QuotationDate <= request.ToDate);
        if (request.Status.HasValue) query = query.Where(q => q.Status == request.Status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(q => q.QuotationDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PurchaseQuotationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PurchaseQuotationDto>>.Success(items, "Purchase quotations retrieved", total);
    }
}

// ── Purchase Order Queries ───────────────────────────────────────────

public record GetPurchaseOrdersQuery(int? VendorId, DateTime? FromDate, DateTime? ToDate, PurchaseDocumentStatus? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PurchaseOrderDto>>>;

public class GetPurchaseOrdersHandler : IRequestHandler<GetPurchaseOrdersQuery, ApiResponse<List<PurchaseOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPurchaseOrdersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PurchaseOrderDto>>> Handle(GetPurchaseOrdersQuery request, CancellationToken ct)
    {
        var query = _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(o => o.VendorId == request.VendorId);
        if (request.FromDate.HasValue) query = query.Where(o => o.OrderDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(o => o.OrderDate <= request.ToDate);
        if (request.Status.HasValue) query = query.Where(o => o.Status == request.Status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PurchaseOrderDto>>.Success(items, "Purchase orders retrieved", total);
    }
}

// ── Receipt Note Queries ─────────────────────────────────────────────

public record GetReceiptNotesQuery(int? VendorId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ReceiptNoteDto>>>;

public class GetReceiptNotesHandler : IRequestHandler<GetReceiptNotesQuery, ApiResponse<List<ReceiptNoteDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetReceiptNotesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ReceiptNoteDto>>> Handle(GetReceiptNotesQuery request, CancellationToken ct)
    {
        var query = _db.ReceiptNotes
            .Include(r => r.Vendor).Include(r => r.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(r => r.VendorId == request.VendorId);
        if (request.FromDate.HasValue) query = query.Where(r => r.ReceiptDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(r => r.ReceiptDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReceiptDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ReceiptNoteDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ReceiptNoteDto>>.Success(items, "Receipt notes retrieved", total);
    }
}

// ── Purchase Return Queries ──────────────────────────────────────────

public record GetPurchaseReturnsQuery(int? VendorId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PurchaseReturnDto>>>;

public class GetPurchaseReturnsHandler : IRequestHandler<GetPurchaseReturnsQuery, ApiResponse<List<PurchaseReturnDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPurchaseReturnsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PurchaseReturnDto>>> Handle(GetPurchaseReturnsQuery request, CancellationToken ct)
    {
        var query = _db.PurchaseReturns
            .Include(r => r.Vendor).Include(r => r.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(r => r.VendorId == request.VendorId);
        if (request.FromDate.HasValue) query = query.Where(r => r.ReturnDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(r => r.ReturnDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReturnDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PurchaseReturnDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PurchaseReturnDto>>.Success(items, "Purchase returns retrieved", total);
    }
}

// ── Pending Purchase Orders Query ────────────────────────────────────

public record GetPendingPurchaseOrdersQuery(int? VendorId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PurchaseOrderDto>>>;

public class GetPendingPurchaseOrdersHandler : IRequestHandler<GetPendingPurchaseOrdersQuery, ApiResponse<List<PurchaseOrderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPendingPurchaseOrdersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PurchaseOrderDto>>> Handle(GetPendingPurchaseOrdersQuery request, CancellationToken ct)
    {
        var query = _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Details).ThenInclude(d => d.Product)
            .Where(o => o.Status == PurchaseDocumentStatus.Approved)
            .Where(o => o.Details.Any(d => d.ReceivedQuantity < d.Quantity))
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(o => o.VendorId == request.VendorId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PurchaseOrderDto>>.Success(items, "Pending purchase orders retrieved", total);
    }
}

// ── Pending Receipts Query ───────────────────────────────────────────

public record GetPendingReceiptsQuery(int? VendorId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ReceiptNoteDto>>>;

public class GetPendingReceiptsHandler : IRequestHandler<GetPendingReceiptsQuery, ApiResponse<List<ReceiptNoteDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPendingReceiptsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ReceiptNoteDto>>> Handle(GetPendingReceiptsQuery request, CancellationToken ct)
    {
        var query = _db.ReceiptNotes
            .Include(r => r.Vendor).Include(r => r.Details).ThenInclude(d => d.Product)
            .Where(r => r.Status == PurchaseDocumentStatus.Pending || r.Status == PurchaseDocumentStatus.Draft)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(r => r.VendorId == request.VendorId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReceiptDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ReceiptNoteDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ReceiptNoteDto>>.Success(items, "Pending receipts retrieved", total);
    }
}
