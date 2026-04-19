using UltimateERP.Application.Features.Inventory.Services;
using static UltimateERP.Application.Features.Inventory.Services.StockCostingService;

namespace UltimateERP.Tests.Inventory;

public class StockCostingServiceTests
{
    [Fact]
    public void CalculateWeightedAverage_ReturnsCorrectAverage()
    {
        var lots = new List<StockLot>
        {
            new(100, 10.00m, DateTime.Today.AddDays(-5)),
            new(200, 12.00m, DateTime.Today.AddDays(-3)),
        };

        var avg = StockCostingService.CalculateWeightedAverage(lots);
        // (100*10 + 200*12) / 300 = 3400/300 = 11.3333
        Assert.Equal(11.3333m, avg);
    }

    [Fact]
    public void CalculateWeightedAverage_EmptyLots_ReturnsZero()
    {
        var avg = StockCostingService.CalculateWeightedAverage(new List<StockLot>());
        Assert.Equal(0, avg);
    }

    [Fact]
    public void CalculateFIFO_IssuesOldestFirst()
    {
        var lots = new List<StockLot>
        {
            new(100, 10.00m, DateTime.Today.AddDays(-5)),
            new(100, 15.00m, DateTime.Today.AddDays(-3)),
            new(100, 20.00m, DateTime.Today.AddDays(-1)),
        };

        var (cogs, remaining) = StockCostingService.CalculateFIFO(lots, 150);

        // FIFO: 100@10 + 50@15 = 1000+750 = 1750
        Assert.Equal(1750m, cogs);
        Assert.Equal(2, remaining.Count);
        Assert.Equal(50, remaining[0].Quantity);
        Assert.Equal(15.00m, remaining[0].Rate);
    }

    [Fact]
    public void CalculateLIFO_IssuesNewestFirst()
    {
        var lots = new List<StockLot>
        {
            new(100, 10.00m, DateTime.Today.AddDays(-5)),
            new(100, 15.00m, DateTime.Today.AddDays(-3)),
            new(100, 20.00m, DateTime.Today.AddDays(-1)),
        };

        var (cogs, remaining) = StockCostingService.CalculateLIFO(lots, 150);

        // LIFO: 100@20 + 50@15 = 2000+750 = 2750
        Assert.Equal(2750m, cogs);
        Assert.Equal(2, remaining.Count);
    }

    [Fact]
    public void CalculateFIFO_ExactQuantity_EmptiesAllLots()
    {
        var lots = new List<StockLot>
        {
            new(50, 10.00m, DateTime.Today.AddDays(-2)),
            new(50, 12.00m, DateTime.Today),
        };

        var (cogs, remaining) = StockCostingService.CalculateFIFO(lots, 100);

        Assert.Equal(1100m, cogs); // 50*10 + 50*12
        Assert.Empty(remaining);
    }

    [Fact]
    public void HasSufficientStock_EnoughStock_ReturnsTrue()
    {
        Assert.True(StockCostingService.HasSufficientStock(100, 50));
    }

    [Fact]
    public void HasSufficientStock_InsufficientStock_ReturnsFalse()
    {
        Assert.False(StockCostingService.HasSufficientStock(30, 50));
    }

    [Fact]
    public void HasSufficientStock_AllowNegative_AlwaysReturnsTrue()
    {
        Assert.True(StockCostingService.HasSufficientStock(0, 50, allowNegative: true));
    }
}
