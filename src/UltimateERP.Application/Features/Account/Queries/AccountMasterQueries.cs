using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Account.Queries;

// ── PaymentTerm Queries ──────────────────────────────────────────────

public record GetPaymentTermsQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PaymentTermDto>>>;

public class GetPaymentTermsHandler : IRequestHandler<GetPaymentTermsQuery, ApiResponse<List<PaymentTermDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPaymentTermsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PaymentTermDto>>> Handle(GetPaymentTermsQuery request, CancellationToken ct)
    {
        var query = _db.PaymentTerms.Where(p => p.IsActive).AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(p => p.Name!.Contains(request.Search) || (p.Code != null && p.Code.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PaymentTermDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PaymentTermDto>>.Success(items, "Payment terms retrieved", total);
    }
}

public record GetPaymentTermByIdQuery(int Id) : IRequest<ApiResponse<PaymentTermDto>>;

public class GetPaymentTermByIdHandler : IRequestHandler<GetPaymentTermByIdQuery, ApiResponse<PaymentTermDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPaymentTermByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentTermDto>> Handle(GetPaymentTermByIdQuery request, CancellationToken ct)
    {
        var entity = await _db.PaymentTerms.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<PaymentTermDto>.Failure("Payment term not found");

        return ApiResponse<PaymentTermDto>.Success(_mapper.Map<PaymentTermDto>(entity), "Payment term retrieved");
    }
}

// ── PaymentMode Queries ──────────────────────────────────────────────

public record GetPaymentModesQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PaymentModeDto>>>;

public class GetPaymentModesHandler : IRequestHandler<GetPaymentModesQuery, ApiResponse<List<PaymentModeDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPaymentModesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PaymentModeDto>>> Handle(GetPaymentModesQuery request, CancellationToken ct)
    {
        var query = _db.PaymentModes.Where(p => p.IsActive).AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(p => p.Name!.Contains(request.Search) || (p.Code != null && p.Code.Contains(request.Search)));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PaymentModeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PaymentModeDto>>.Success(items, "Payment modes retrieved", total);
    }
}

public record GetPaymentModeByIdQuery(int Id) : IRequest<ApiResponse<PaymentModeDto>>;

public class GetPaymentModeByIdHandler : IRequestHandler<GetPaymentModeByIdQuery, ApiResponse<PaymentModeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPaymentModeByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentModeDto>> Handle(GetPaymentModeByIdQuery request, CancellationToken ct)
    {
        var entity = await _db.PaymentModes.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<PaymentModeDto>.Failure("Payment mode not found");

        return ApiResponse<PaymentModeDto>.Success(_mapper.Map<PaymentModeDto>(entity), "Payment mode retrieved");
    }
}
