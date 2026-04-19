using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Features.Setup.Services;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Setup.Queries;

// ── Currency Queries ──────────────────────────────────────────────────

public record GetCurrenciesQuery(string? Search) : IRequest<ApiResponse<List<CurrencyDto>>>;

public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesQuery, ApiResponse<List<CurrencyDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetCurrenciesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<CurrencyDto>>> Handle(GetCurrenciesQuery request, CancellationToken ct)
    {
        var query = _db.Currencies.Where(c => c.IsActive);
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(c => c.Name.Contains(request.Search) || c.CurrencyCode.Contains(request.Search));

        var items = await query
            .OrderBy(c => c.CurrencyCode)
            .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<CurrencyDto>>.Success(items, "Currencies retrieved", items.Count);
    }
}

// ── Exchange Rate Queries ─────────────────────────────────────────────

public record GetExchangeRatesQuery(int? FromCurrencyId, int? ToCurrencyId) : IRequest<ApiResponse<List<ExchangeRateDto>>>;

public class GetExchangeRatesHandler : IRequestHandler<GetExchangeRatesQuery, ApiResponse<List<ExchangeRateDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetExchangeRatesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<ExchangeRateDto>>> Handle(GetExchangeRatesQuery request, CancellationToken ct)
    {
        var query = _db.ExchangeRates
            .Include(r => r.FromCurrency)
            .Include(r => r.ToCurrency)
            .Where(r => r.IsActive);

        if (request.FromCurrencyId.HasValue)
            query = query.Where(r => r.FromCurrencyId == request.FromCurrencyId.Value);
        if (request.ToCurrencyId.HasValue)
            query = query.Where(r => r.ToCurrencyId == request.ToCurrencyId.Value);

        var items = await query
            .OrderByDescending(r => r.EffectiveDate)
            .ProjectTo<ExchangeRateDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<ExchangeRateDto>>.Success(items, "Exchange rates retrieved", items.Count);
    }
}

// ── Convert Currency Query ────────────────────────────────────────────

public record ConvertCurrencyQuery(decimal Amount, int FromCurrencyId, int ToCurrencyId, DateTime? AsOfDate = null)
    : IRequest<ApiResponse<CurrencyConversionResultDto>>;

public class ConvertCurrencyHandler : IRequestHandler<ConvertCurrencyQuery, ApiResponse<CurrencyConversionResultDto>>
{
    private readonly ICurrencyConversionService _conversionService;

    public ConvertCurrencyHandler(ICurrencyConversionService conversionService) { _conversionService = conversionService; }

    public async Task<ApiResponse<CurrencyConversionResultDto>> Handle(ConvertCurrencyQuery request, CancellationToken ct)
    {
        return await _conversionService.ConvertAsync(request.Amount, request.FromCurrencyId, request.ToCurrencyId, request.AsOfDate, ct);
    }
}
