using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Industry.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Industry.Queries;

// ── Get Dairy Purchases ───────────────────────────────────────────────

public record GetDairyPurchasesQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<DairyPurchaseInvoiceDto>>>;

public class GetDairyPurchasesHandler : IRequestHandler<GetDairyPurchasesQuery, ApiResponse<List<DairyPurchaseInvoiceDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetDairyPurchasesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<DairyPurchaseInvoiceDto>>> Handle(GetDairyPurchasesQuery request, CancellationToken ct)
    {
        var query = _db.DairyPurchaseInvoices
            .Include(d => d.Vendor)
            .Where(d => d.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(d => d.InvoiceNumber.Contains(request.Search)
                || (d.Vendor != null && d.Vendor.Name != null && d.Vendor.Name.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(d => d.InvoiceDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DairyPurchaseInvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<DairyPurchaseInvoiceDto>>.Success(items, "Dairy purchases retrieved", total);
    }
}

// ── Get Tea Purchases ─────────────────────────────────────────────────

public record GetTeaPurchasesQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<TeaPurchaseInvoiceDto>>>;

public class GetTeaPurchasesHandler : IRequestHandler<GetTeaPurchasesQuery, ApiResponse<List<TeaPurchaseInvoiceDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetTeaPurchasesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<TeaPurchaseInvoiceDto>>> Handle(GetTeaPurchasesQuery request, CancellationToken ct)
    {
        var query = _db.TeaPurchaseInvoices
            .Include(t => t.Vendor)
            .Where(t => t.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(t => t.InvoiceNumber.Contains(request.Search)
                || (t.Vendor != null && t.Vendor.Name != null && t.Vendor.Name.Contains(request.Search))
                || (t.GardenName != null && t.GardenName.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.InvoiceDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<TeaPurchaseInvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<TeaPurchaseInvoiceDto>>.Success(items, "Tea purchases retrieved", total);
    }
}

// ── Get Petrol Pump Transactions ──────────────────────────────────────

public record GetPetrolPumpTransactionsQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PetrolPumpTransactionDto>>>;

public class GetPetrolPumpTransactionsHandler : IRequestHandler<GetPetrolPumpTransactionsQuery, ApiResponse<List<PetrolPumpTransactionDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPetrolPumpTransactionsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PetrolPumpTransactionDto>>> Handle(GetPetrolPumpTransactionsQuery request, CancellationToken ct)
    {
        var query = _db.PetrolPumpTransactions
            .Include(t => t.Product)
            .Include(t => t.Customer)
            .Where(t => t.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(t => t.TransactionNumber.Contains(request.Search)
                || (t.Product != null && t.Product.Name != null && t.Product.Name.Contains(request.Search))
                || (t.Customer != null && t.Customer.Name != null && t.Customer.Name.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PetrolPumpTransactionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PetrolPumpTransactionDto>>.Success(items, "Petrol pump transactions retrieved", total);
    }
}
