using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Application.Features.Setup.Commands;

// ── Currency Commands ─────────────────────────────────────────────────

public record CreateCurrencyCommand(CreateCurrencyDto Currency) : IRequest<ApiResponse<CurrencyDto>>;

public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyValidator()
    {
        RuleFor(x => x.Currency.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Currency.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Currency.CurrencyCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Currency.DecimalPlaces).InclusiveBetween(0, 6);
    }
}

public class CreateCurrencyHandler : IRequestHandler<CreateCurrencyCommand, ApiResponse<CurrencyDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateCurrencyHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<CurrencyDto>> Handle(CreateCurrencyCommand request, CancellationToken ct)
    {
        if (await _db.Currencies.AnyAsync(c => c.CurrencyCode == request.Currency.CurrencyCode, ct))
            return ApiResponse<CurrencyDto>.Failure($"Currency code '{request.Currency.CurrencyCode}' already exists");

        if (request.Currency.IsBaseCurrency)
        {
            var existingBase = await _db.Currencies.FirstOrDefaultAsync(c => c.IsBaseCurrency && c.IsActive, ct);
            if (existingBase is not null)
                return ApiResponse<CurrencyDto>.Failure("A base currency already exists. Update the existing one first.");
        }

        var entity = _mapper.Map<Currency>(request.Currency);
        _db.Currencies.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<CurrencyDto>.Success(_mapper.Map<CurrencyDto>(entity), "Currency created");
    }
}

// ── Exchange Rate Commands ────────────────────────────────────────────

public record SetExchangeRateCommand(CreateExchangeRateDto ExchangeRate) : IRequest<ApiResponse<ExchangeRateDto>>;

public class SetExchangeRateValidator : AbstractValidator<SetExchangeRateCommand>
{
    public SetExchangeRateValidator()
    {
        RuleFor(x => x.ExchangeRate.FromCurrencyId).GreaterThan(0);
        RuleFor(x => x.ExchangeRate.ToCurrencyId).GreaterThan(0);
        RuleFor(x => x.ExchangeRate.Rate).GreaterThan(0);
        RuleFor(x => x.ExchangeRate)
            .Must(x => x.FromCurrencyId != x.ToCurrencyId)
            .WithMessage("From and To currencies must be different");
    }
}

public class SetExchangeRateHandler : IRequestHandler<SetExchangeRateCommand, ApiResponse<ExchangeRateDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public SetExchangeRateHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ExchangeRateDto>> Handle(SetExchangeRateCommand request, CancellationToken ct)
    {
        var fromCurrency = await _db.Currencies.FindAsync(new object[] { request.ExchangeRate.FromCurrencyId }, ct);
        if (fromCurrency is null) return ApiResponse<ExchangeRateDto>.Failure("From currency not found");

        var toCurrency = await _db.Currencies.FindAsync(new object[] { request.ExchangeRate.ToCurrencyId }, ct);
        if (toCurrency is null) return ApiResponse<ExchangeRateDto>.Failure("To currency not found");

        var entity = _mapper.Map<ExchangeRate>(request.ExchangeRate);
        entity.Name = $"{fromCurrency.CurrencyCode}/{toCurrency.CurrencyCode}";
        entity.Code = $"{fromCurrency.CurrencyCode}-{toCurrency.CurrencyCode}-{request.ExchangeRate.EffectiveDate:yyyyMMdd}";
        _db.ExchangeRates.Add(entity);
        await _db.SaveChangesAsync(ct);

        var dto = _mapper.Map<ExchangeRateDto>(entity);
        dto.FromCurrencyCode = fromCurrency.CurrencyCode;
        dto.ToCurrencyCode = toCurrency.CurrencyCode;

        return ApiResponse<ExchangeRateDto>.Success(dto, "Exchange rate set");
    }
}
