using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Account.Queries;

// ── LedgerGroup Queries ───────────────────────────────────────────────

public record GetLedgerGroupTreeQuery : IRequest<ApiResponse<List<LedgerGroupDto>>>;

public class GetLedgerGroupTreeHandler : IRequestHandler<GetLedgerGroupTreeQuery, ApiResponse<List<LedgerGroupDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetLedgerGroupTreeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LedgerGroupDto>>> Handle(GetLedgerGroupTreeQuery request, CancellationToken ct)
    {
        var allGroups = await _db.LedgerGroups
            .Include(g => g.ParentGroup)
            .Where(g => g.IsActive)
            .ProjectTo<LedgerGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        // Build tree: root groups are those without a parent
        var lookup = allGroups.ToLookup(g => g.ParentGroupId);
        var roots = allGroups.Where(g => g.ParentGroupId == null).ToList();

        void BuildTree(LedgerGroupDto node)
        {
            node.Children = lookup[node.Id].ToList();
            foreach (var child in node.Children)
                BuildTree(child);
        }

        foreach (var root in roots)
            BuildTree(root);

        return ApiResponse<List<LedgerGroupDto>>.Success(roots, "Ledger group tree retrieved", allGroups.Count);
    }
}

// ── Ledger Queries ────────────────────────────────────────────────────

public record GetLedgersQuery(string? Search, int? GroupId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<LedgerDto>>>;

public class GetLedgersHandler : IRequestHandler<GetLedgersQuery, ApiResponse<List<LedgerDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetLedgersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LedgerDto>>> Handle(GetLedgersQuery request, CancellationToken ct)
    {
        var query = _db.Ledgers.Include(l => l.LedgerGroup).Where(l => l.IsActive).AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(l => l.Name.Contains(request.Search) || l.Code.Contains(request.Search));
        if (request.GroupId.HasValue)
            query = query.Where(l => l.LedgerGroupId == request.GroupId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(l => l.Code)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LedgerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LedgerDto>>.Success(items, "Ledgers retrieved", total);
    }
}

// ── Voucher Queries ───────────────────────────────────────────────────

public record GetVouchersQuery(DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<VoucherDto>>>;

public class GetVouchersHandler : IRequestHandler<GetVouchersQuery, ApiResponse<List<VoucherDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetVouchersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<VoucherDto>>> Handle(GetVouchersQuery request, CancellationToken ct)
    {
        var query = _db.Vouchers.Include(v => v.Details).ThenInclude(d => d.Ledger).AsQueryable();

        if (request.FromDate.HasValue)
            query = query.Where(v => v.VoucherDate >= request.FromDate.Value);
        if (request.ToDate.HasValue)
            query = query.Where(v => v.VoucherDate <= request.ToDate.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(v => v.VoucherDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<VoucherDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<VoucherDto>>.Success(items, "Vouchers retrieved", total);
    }
}

// ── Customer Queries ──────────────────────────────────────────────────

public record GetCustomersQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<CustomerDto>>>;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, ApiResponse<List<CustomerDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetCustomersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken ct)
    {
        var query = _db.Customers.Include(c => c.Ledger).Where(c => c.IsActive).AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(c => c.Name.Contains(request.Search) || c.Code.Contains(request.Search));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(c => c.Code)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<CustomerDto>>.Success(items, "Customers retrieved", total);
    }
}

// ── Vendor Queries ────────────────────────────────────────────────────

public record GetVendorsQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<VendorDto>>>;

public class GetVendorsHandler : IRequestHandler<GetVendorsQuery, ApiResponse<List<VendorDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetVendorsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<VendorDto>>> Handle(GetVendorsQuery request, CancellationToken ct)
    {
        var query = _db.Vendors.Include(v => v.Ledger).Where(v => v.IsActive).AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(v => v.Name.Contains(request.Search) || v.Code.Contains(request.Search));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(v => v.Code)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<VendorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<VendorDto>>.Success(items, "Vendors retrieved", total);
    }
}
