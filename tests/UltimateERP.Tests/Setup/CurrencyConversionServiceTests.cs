using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Setup.Services;

namespace UltimateERP.Tests.Setup;

public class CurrencyConversionServiceTests
{
    private readonly CurrencyConversionService _service;

    public CurrencyConversionServiceTests()
    {
        // Use a simple in-memory context for pure method tests
        _service = CreateService();
    }

    private static CurrencyConversionService CreateService()
    {
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
            UltimateERP.Infrastructure.Persistence.ERPDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UltimateERP.Infrastructure.Persistence.ERPDbContext(options);
        return new CurrencyConversionService(context);
    }

    [Fact]
    public void Convert_MultipliesAmountByRate()
    {
        var result = _service.Convert(100m, 1.35m);
        Assert.Equal(135m, result);
    }

    [Fact]
    public void Convert_HandlesDecimalPrecision()
    {
        var result = _service.Convert(1000m, 0.008333m);
        Assert.Equal(8.333m, result);
    }

    [Fact]
    public void GetInverseRate_ReturnsReciprocal()
    {
        var rate = _service.GetInverseRate(2m);
        Assert.Equal(0.5m, rate);
    }

    [Fact]
    public void GetInverseRate_HandlesDecimalRate()
    {
        var rate = _service.GetInverseRate(0.5m);
        Assert.Equal(2m, rate);
    }

    [Fact]
    public void GetInverseRate_ZeroRate_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => _service.GetInverseRate(0m));
    }

    [Fact]
    public async Task ConvertAsync_SameCurrency_ReturnsOriginalAmount()
    {
        // Arrange
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
            UltimateERP.Infrastructure.Persistence.ERPDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UltimateERP.Infrastructure.Persistence.ERPDbContext(options);

        var currency = new UltimateERP.Domain.Entities.Setup.Currency
        {
            Code = "NPR",
            Name = "Nepalese Rupee",
            CurrencyCode = "NPR",
            IsActive = true
        };
        context.Currencies.Add(currency);
        await context.SaveChangesAsync();

        var service = new CurrencyConversionService(context);

        // Act
        var result = await service.ConvertAsync(500m, currency.Id, currency.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(500m, result.Data!.ConvertedAmount);
        Assert.Equal(1m, result.Data.ExchangeRate);
    }

    [Fact]
    public async Task ConvertAsync_DirectRate_ConvertsCorrectly()
    {
        // Arrange
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
            UltimateERP.Infrastructure.Persistence.ERPDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UltimateERP.Infrastructure.Persistence.ERPDbContext(options);

        var usd = new UltimateERP.Domain.Entities.Setup.Currency { Code = "USD", Name = "US Dollar", CurrencyCode = "USD", IsActive = true };
        var npr = new UltimateERP.Domain.Entities.Setup.Currency { Code = "NPR", Name = "Nepalese Rupee", CurrencyCode = "NPR", IsActive = true };
        context.Currencies.AddRange(usd, npr);
        await context.SaveChangesAsync();

        var rate = new UltimateERP.Domain.Entities.Setup.ExchangeRate
        {
            FromCurrencyId = usd.Id,
            ToCurrencyId = npr.Id,
            Rate = 133.5m,
            EffectiveDate = DateTime.UtcNow.AddDays(-1),
            IsActive = true
        };
        context.ExchangeRates.Add(rate);
        await context.SaveChangesAsync();

        var service = new CurrencyConversionService(context);

        // Act
        var result = await service.ConvertAsync(100m, usd.Id, npr.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(13350m, result.Data!.ConvertedAmount);
        Assert.Equal(133.5m, result.Data.ExchangeRate);
    }

    [Fact]
    public async Task ConvertAsync_InverseRate_ConvertsCorrectly()
    {
        // Arrange
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
            UltimateERP.Infrastructure.Persistence.ERPDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UltimateERP.Infrastructure.Persistence.ERPDbContext(options);

        var usd = new UltimateERP.Domain.Entities.Setup.Currency { Code = "USD", Name = "US Dollar", CurrencyCode = "USD", IsActive = true };
        var npr = new UltimateERP.Domain.Entities.Setup.Currency { Code = "NPR", Name = "Nepalese Rupee", CurrencyCode = "NPR", IsActive = true };
        context.Currencies.AddRange(usd, npr);
        await context.SaveChangesAsync();

        // Only define rate from USD to NPR, but query NPR to USD
        var rate = new UltimateERP.Domain.Entities.Setup.ExchangeRate
        {
            FromCurrencyId = usd.Id,
            ToCurrencyId = npr.Id,
            Rate = 133.5m,
            EffectiveDate = DateTime.UtcNow.AddDays(-1),
            IsActive = true
        };
        context.ExchangeRates.Add(rate);
        await context.SaveChangesAsync();

        var service = new CurrencyConversionService(context);

        // Act - convert NPR to USD (inverse)
        var result = await service.ConvertAsync(13350m, npr.Id, usd.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data!.ConvertedAmount > 99m && result.Data.ConvertedAmount < 101m);
    }

    [Fact]
    public async Task ConvertAsync_MissingRate_ReturnsFailure()
    {
        // Arrange
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
            UltimateERP.Infrastructure.Persistence.ERPDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new UltimateERP.Infrastructure.Persistence.ERPDbContext(options);

        var usd = new UltimateERP.Domain.Entities.Setup.Currency { Code = "USD", Name = "US Dollar", CurrencyCode = "USD", IsActive = true };
        var eur = new UltimateERP.Domain.Entities.Setup.Currency { Code = "EUR", Name = "Euro", CurrencyCode = "EUR", IsActive = true };
        context.Currencies.AddRange(usd, eur);
        await context.SaveChangesAsync();

        var service = new CurrencyConversionService(context);

        // Act - no rate defined between USD and EUR
        var result = await service.ConvertAsync(100m, usd.Id, eur.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No exchange rate found", result.ResponseMSG);
    }
}
