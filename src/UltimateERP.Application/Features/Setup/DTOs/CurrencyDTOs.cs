namespace UltimateERP.Application.Features.Setup.DTOs;

// ── Branch Filter ─────────────────────────────────────────────────────

public class BranchFilterDto
{
    public int? BranchId { get; set; }
}

// ── Currency ──────────────────────────────────────────────────────────

public class CurrencyDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public int DecimalPlaces { get; set; }
    public bool IsBaseCurrency { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCurrencyDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public int DecimalPlaces { get; set; } = 2;
    public bool IsBaseCurrency { get; set; }
}

// ── Exchange Rate ─────────────────────────────────────────────────────

public class ExchangeRateDto
{
    public int Id { get; set; }
    public int FromCurrencyId { get; set; }
    public string? FromCurrencyCode { get; set; }
    public int ToCurrencyId { get; set; }
    public string? ToCurrencyCode { get; set; }
    public decimal Rate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateExchangeRateDto
{
    public int FromCurrencyId { get; set; }
    public int ToCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public DateTime EffectiveDate { get; set; }
}

// ── Currency Conversion ───────────────────────────────────────────────

public class CurrencyConversionResultDto
{
    public decimal OriginalAmount { get; set; }
    public string FromCurrencyCode { get; set; } = string.Empty;
    public string ToCurrencyCode { get; set; } = string.Empty;
    public decimal ConvertedAmount { get; set; }
    public decimal ExchangeRate { get; set; }
    public DateTime RateDate { get; set; }
}
