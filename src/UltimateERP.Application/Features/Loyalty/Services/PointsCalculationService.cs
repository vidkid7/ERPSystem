namespace UltimateERP.Application.Features.Loyalty.Services;

public static class PointsCalculationService
{
    public static decimal CalculatePoints(decimal saleAmount, string tier)
    {
        var rate = tier switch
        {
            "Bronze" => 1m,
            "Silver" => 1.5m,
            "Gold" => 2m,
            "Platinum" => 3m,
            _ => 1m
        };

        return Math.Round(saleAmount / 100m * rate, 2);
    }
}
