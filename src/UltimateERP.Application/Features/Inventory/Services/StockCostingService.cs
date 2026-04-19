using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Inventory.Services;

/// <summary>
/// Stock costing calculator supporting FIFO, LIFO, Weighted Average, and Specific methods.
/// </summary>
public static class StockCostingService
{
    public record StockLot(decimal Quantity, decimal Rate, DateTime Date);

    public static decimal CalculateWeightedAverage(IEnumerable<StockLot> lots)
    {
        var totalQty = lots.Sum(l => l.Quantity);
        if (totalQty == 0) return 0;
        var totalValue = lots.Sum(l => l.Quantity * l.Rate);
        return Math.Round(totalValue / totalQty, 4);
    }

    public static (decimal CostOfGoodsSold, List<StockLot> RemainingLots) CalculateFIFO(
        List<StockLot> lots, decimal quantityToIssue)
    {
        var remaining = lots.OrderBy(l => l.Date).ToList();
        decimal cogs = 0;
        decimal toIssue = quantityToIssue;

        for (int i = 0; i < remaining.Count && toIssue > 0; i++)
        {
            var lot = remaining[i];
            var issued = Math.Min(lot.Quantity, toIssue);
            cogs += issued * lot.Rate;
            toIssue -= issued;
            remaining[i] = lot with { Quantity = lot.Quantity - issued };
        }

        remaining.RemoveAll(l => l.Quantity <= 0);
        return (cogs, remaining);
    }

    public static (decimal CostOfGoodsSold, List<StockLot> RemainingLots) CalculateLIFO(
        List<StockLot> lots, decimal quantityToIssue)
    {
        var remaining = lots.OrderByDescending(l => l.Date).ToList();
        decimal cogs = 0;
        decimal toIssue = quantityToIssue;

        for (int i = 0; i < remaining.Count && toIssue > 0; i++)
        {
            var lot = remaining[i];
            var issued = Math.Min(lot.Quantity, toIssue);
            cogs += issued * lot.Rate;
            toIssue -= issued;
            remaining[i] = lot with { Quantity = lot.Quantity - issued };
        }

        remaining.RemoveAll(l => l.Quantity <= 0);
        return (cogs, remaining);
    }

    public static bool HasSufficientStock(decimal currentQuantity, decimal requiredQuantity, bool allowNegative = false)
    {
        if (allowNegative) return true;
        return currentQuantity >= requiredQuantity;
    }
}
