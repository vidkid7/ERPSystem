using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UltimateERP.Application.Features.Setup.Services;

public interface ICurrencyConversionService
{
    Task<ApiResponse<CurrencyConversionResultDto>> ConvertAsync(
        decimal amount, int fromCurrencyId, int toCurrencyId, DateTime? asOfDate = null, CancellationToken ct = default);

    decimal Convert(decimal amount, decimal exchangeRate);
    decimal GetInverseRate(decimal rate);
}

public class CurrencyConversionService : ICurrencyConversionService
{
    private readonly IApplicationDbContext _db;

    public CurrencyConversionService(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<CurrencyConversionResultDto>> ConvertAsync(
        decimal amount, int fromCurrencyId, int toCurrencyId, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        if (fromCurrencyId == toCurrencyId)
        {
            var currency = await _db.Currencies.FindAsync(new object[] { fromCurrencyId }, ct);
            if (currency is null) return ApiResponse<CurrencyConversionResultDto>.Failure("Currency not found");

            return ApiResponse<CurrencyConversionResultDto>.Success(new CurrencyConversionResultDto
            {
                OriginalAmount = amount,
                FromCurrencyCode = currency.CurrencyCode,
                ToCurrencyCode = currency.CurrencyCode,
                ConvertedAmount = amount,
                ExchangeRate = 1m,
                RateDate = asOfDate ?? DateTime.UtcNow
            });
        }

        var effectiveDate = asOfDate ?? DateTime.UtcNow;

        // Try direct rate
        var rate = await _db.ExchangeRates
            .Where(r => r.FromCurrencyId == fromCurrencyId && r.ToCurrencyId == toCurrencyId
                        && r.EffectiveDate <= effectiveDate && r.IsActive)
            .OrderByDescending(r => r.EffectiveDate)
            .FirstOrDefaultAsync(ct);

        if (rate is not null)
        {
            var fromCurrency = await _db.Currencies.FindAsync(new object[] { fromCurrencyId }, ct);
            var toCurrency = await _db.Currencies.FindAsync(new object[] { toCurrencyId }, ct);

            return ApiResponse<CurrencyConversionResultDto>.Success(new CurrencyConversionResultDto
            {
                OriginalAmount = amount,
                FromCurrencyCode = fromCurrency?.CurrencyCode ?? string.Empty,
                ToCurrencyCode = toCurrency?.CurrencyCode ?? string.Empty,
                ConvertedAmount = Convert(amount, rate.Rate),
                ExchangeRate = rate.Rate,
                RateDate = rate.EffectiveDate
            });
        }

        // Try inverse rate
        var inverseRate = await _db.ExchangeRates
            .Where(r => r.FromCurrencyId == toCurrencyId && r.ToCurrencyId == fromCurrencyId
                        && r.EffectiveDate <= effectiveDate && r.IsActive)
            .OrderByDescending(r => r.EffectiveDate)
            .FirstOrDefaultAsync(ct);

        if (inverseRate is not null)
        {
            var fromCurrency = await _db.Currencies.FindAsync(new object[] { fromCurrencyId }, ct);
            var toCurrency = await _db.Currencies.FindAsync(new object[] { toCurrencyId }, ct);
            var calcRate = GetInverseRate(inverseRate.Rate);

            return ApiResponse<CurrencyConversionResultDto>.Success(new CurrencyConversionResultDto
            {
                OriginalAmount = amount,
                FromCurrencyCode = fromCurrency?.CurrencyCode ?? string.Empty,
                ToCurrencyCode = toCurrency?.CurrencyCode ?? string.Empty,
                ConvertedAmount = Convert(amount, calcRate),
                ExchangeRate = calcRate,
                RateDate = inverseRate.EffectiveDate
            });
        }

        return ApiResponse<CurrencyConversionResultDto>.Failure(
            "No exchange rate found for the specified currency pair and date");
    }

    public decimal Convert(decimal amount, decimal exchangeRate)
    {
        return Math.Round(amount * exchangeRate, 6);
    }

    public decimal GetInverseRate(decimal rate)
    {
        if (rate == 0) throw new InvalidOperationException("Exchange rate cannot be zero");
        return Math.Round(1m / rate, 6);
    }
}
