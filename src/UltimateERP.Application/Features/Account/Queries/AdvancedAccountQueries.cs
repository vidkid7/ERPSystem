using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Account.Queries;

// ── PDC Queries ───────────────────────────────────────────────────────

public record GetPDCsQuery(PDCStatus? Status, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PDCDto>>>;

public class GetPDCsHandler : IRequestHandler<GetPDCsQuery, ApiResponse<List<PDCDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPDCsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PDCDto>>> Handle(GetPDCsQuery request, CancellationToken ct)
    {
        var query = _db.PDCs.Include(p => p.Ledger).Where(p => p.IsActive).AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(p => p.Status == request.Status.Value);
        if (request.FromDate.HasValue)
            query = query.Where(p => p.ChequeDate >= request.FromDate.Value);
        if (request.ToDate.HasValue)
            query = query.Where(p => p.ChequeDate <= request.ToDate.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.ChequeDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PDCDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PDCDto>>.Success(items, "PDCs retrieved", total);
    }
}

// ── ODC Queries ───────────────────────────────────────────────────────

public record GetODCsQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ODCDto>>>;

public class GetODCsHandler : IRequestHandler<GetODCsQuery, ApiResponse<List<ODCDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetODCsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ODCDto>>> Handle(GetODCsQuery request, CancellationToken ct)
    {
        var query = _db.ODCs.Include(o => o.Ledger).Where(o => o.IsActive).AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(o => o.Status == request.Status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(o => o.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ODCDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ODCDto>>.Success(items, "ODCs retrieved", total);
    }
}

// ── Bank Guarantee Queries ────────────────────────────────────────────

public record GetBankGuaranteesQuery(string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<BankGuaranteeDto>>>;

public class GetBankGuaranteesHandler : IRequestHandler<GetBankGuaranteesQuery, ApiResponse<List<BankGuaranteeDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetBankGuaranteesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BankGuaranteeDto>>> Handle(GetBankGuaranteesQuery request, CancellationToken ct)
    {
        var query = _db.BankGuarantees.Include(bg => bg.Ledger).Where(bg => bg.IsActive).AsQueryable();

        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(bg => bg.Status == request.Status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(bg => bg.ValidFrom)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<BankGuaranteeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BankGuaranteeDto>>.Success(items, "Bank guarantees retrieved", total);
    }
}

// ── Letter of Credit Queries ──────────────────────────────────────────

public record GetLettersOfCreditQuery(LCStatus? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LetterOfCreditDto>>>;

public class GetLettersOfCreditHandler : IRequestHandler<GetLettersOfCreditQuery, ApiResponse<List<LetterOfCreditDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetLettersOfCreditHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LetterOfCreditDto>>> Handle(GetLettersOfCreditQuery request, CancellationToken ct)
    {
        var query = _db.LettersOfCredit.Include(lc => lc.Vendor).Where(lc => lc.IsActive).AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(lc => lc.Status == request.Status.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(lc => lc.OpeningDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LetterOfCreditDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LetterOfCreditDto>>.Success(items, "Letters of credit retrieved", total);
    }
}

// ── Bank Reconciliation Queries ───────────────────────────────────────

public record GetUnreconciledTransactionsQuery(int LedgerId, int Page = 1, int PageSize = 50)
    : IRequest<ApiResponse<List<BankReconciliationDto>>>;

public class GetUnreconciledTransactionsHandler : IRequestHandler<GetUnreconciledTransactionsQuery, ApiResponse<List<BankReconciliationDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetUnreconciledTransactionsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BankReconciliationDto>>> Handle(GetUnreconciledTransactionsQuery request, CancellationToken ct)
    {
        var query = _db.BankReconciliations
            .Include(r => r.Ledger)
            .Where(r => r.LedgerId == request.LedgerId && !r.IsReconciled && r.IsActive);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(r => r.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<BankReconciliationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BankReconciliationDto>>.Success(items, "Unreconciled transactions retrieved", total);
    }
}
