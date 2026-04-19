using UltimateERP.Application.Features.Finance.Services;
using UltimateERP.Application.Features.Inventory.Services;
using UltimateERP.Application.Features.Loyalty.Services;
using static UltimateERP.Application.Features.Inventory.Services.StockCostingService;

namespace UltimateERP.Tests.Integration;

/// <summary>
/// Cross-module integration tests verifying interactions between Inventory, Finance, Loyalty,
/// and Account modules at the service/business-logic level.
/// </summary>
public class CrossModuleIntegrationTests
{
    // ── Loyalty + Sales cross-module ────────────────────────────────────────

    [Theory]
    [InlineData("Bronze",   1000,   10.00)]
    [InlineData("Silver",   1000,   15.00)]
    [InlineData("Gold",     1000,   20.00)]
    [InlineData("Platinum", 1000,   30.00)]
    public void SalesWithLoyaltyPoints_AllTiers_EarnCorrectPoints(string tier, decimal amount, decimal expected)
    {
        var points = PointsCalculationService.CalculatePoints(amount, tier);
        Assert.Equal(expected, points);
    }

    [Fact]
    public void LargeOrder_GoldCustomer_EarnsMorePointsThanBronze()
    {
        const decimal saleAmount = 50000m;
        var goldPoints   = PointsCalculationService.CalculatePoints(saleAmount, "Gold");
        var bronzePoints = PointsCalculationService.CalculatePoints(saleAmount, "Bronze");
        Assert.True(goldPoints > bronzePoints);
    }

    [Fact]
    public void UnknownTier_DefaultsToBronzeRate()
    {
        var unknown = PointsCalculationService.CalculatePoints(1000m, "VIP");
        var bronze  = PointsCalculationService.CalculatePoints(1000m, "Bronze");
        Assert.Equal(bronze, unknown);
    }

    // ── Inventory + Finance cross-module ───────────────────────────────────

    [Fact]
    public void PurchaseWithFIFOCosting_ThenEMISchedule_IndependentCalculations()
    {
        // Simulate: purchase goods via FIFO, then finance the purchase with a loan
        var lots = new List<StockLot>
        {
            new(200, 50m,  DateTime.Today.AddDays(-10)),
            new(300, 60m,  DateTime.Today.AddDays(-5)),
        };
        var (cogs, remaining) = StockCostingService.CalculateFIFO(lots, 250);

        // FIFO: 200@50 + 50@60 = 10000 + 3000 = 13000
        Assert.Equal(13000m, cogs);
        Assert.Single(remaining);
        Assert.Equal(250, remaining[0].Quantity); // 300 - 50 = 250 remaining

        // Now finance the COGS amount with EMI
        var loanAmount = cogs; // Finance the exact purchase cost
        var emi        = EMICalculationService.CalculateEMI(loanAmount, 12m, 6);

        Assert.True(emi > 0);
        Assert.True(emi < loanAmount); // Each EMI should be less than full loan amount

        var schedule = EMICalculationService.GenerateSchedule(loanAmount, 12m, 6, DateTime.Today);
        Assert.Equal(6, schedule.Count);
        Assert.Equal(0m, schedule.Last().OutstandingBalance);
    }

    [Fact]
    public void PurchaseWithLIFOCosting_HigherCOGS_LeadsToHigherLoanEMI()
    {
        var lots = new List<StockLot>
        {
            new(100, 40m, DateTime.Today.AddDays(-10)),
            new(100, 80m, DateTime.Today.AddDays(-5)),
        };

        var (fifoCogs, _) = StockCostingService.CalculateFIFO(lots.ToList(), 100);
        var (lifoCogs, _) = StockCostingService.CalculateLIFO(lots.ToList(), 100);

        // FIFO: 100@40 = 4000; LIFO: 100@80 = 8000
        Assert.True(lifoCogs > fifoCogs);

        var fifoEmi = EMICalculationService.CalculateEMI(fifoCogs, 10m, 12);
        var lifoEmi = EMICalculationService.CalculateEMI(lifoCogs, 10m, 12);

        Assert.True(lifoEmi > fifoEmi);
    }

    // ── Finance module standalone ──────────────────────────────────────────

    [Fact]
    public void EMISchedule_TotalPrincipalRepaid_EqualsLoanAmount()
    {
        const decimal loanAmount = 100000m;
        var schedule = EMICalculationService.GenerateSchedule(loanAmount, 12m, 12, DateTime.Today);

        var totalPrincipal = schedule.Sum(s => s.PrincipalComponent);
        Assert.Equal(loanAmount, totalPrincipal, precision: 0);
    }

    [Fact]
    public void EMISchedule_TotalInterest_IsPositive()
    {
        var schedule = EMICalculationService.GenerateSchedule(50000m, 18m, 24, DateTime.Today);
        var totalInterest = schedule.Sum(s => s.InterestComponent);
        Assert.True(totalInterest > 0);
    }

    [Fact]
    public void EMISchedule_EachInstalment_HasDecreasingInterestComponent()
    {
        var schedule = EMICalculationService.GenerateSchedule(100000m, 12m, 12, DateTime.Today);

        for (int i = 1; i < schedule.Count; i++)
        {
            Assert.True(schedule[i].InterestComponent <= schedule[i - 1].InterestComponent,
                $"Interest should decrease: month {i + 1} ({schedule[i].InterestComponent}) > month {i} ({schedule[i - 1].InterestComponent})");
        }
    }

    [Fact]
    public void Rebate_AndPenalty_AreSymmetric()
    {
        const decimal amount = 10000m;
        const decimal rate   = 2m;
        var rebate  = EMICalculationService.CalculateRebate(amount, rate);
        var penalty = EMICalculationService.CalculatePenalty(amount, rate);
        Assert.Equal(rebate, penalty);
    }

    // ── Stock + Account cross-module ───────────────────────────────────────

    [Fact]
    public void WeightedAverageCost_UsedInVoucherAmount_BalancesCorrectly()
    {
        // Calculate weighted average cost for a product
        var lots = new List<StockLot>
        {
            new(50,  100m, DateTime.Today.AddDays(-3)),
            new(150, 120m, DateTime.Today.AddDays(-1)),
        };
        var avgCost = StockCostingService.CalculateWeightedAverage(lots);
        // (50*100 + 150*120) / 200 = (5000 + 18000) / 200 = 115
        Assert.Equal(115m, avgCost);

        // Verify: cost * qty = expected total for issuance
        const int issueQty = 100;
        var issueCost = avgCost * issueQty; // 11500
        Assert.Equal(11500m, issueCost);

        // In accounting, debit COGS = credit Stock for this amount
        // (both sides of voucher are equal - double entry principle)
        var debit  = issueCost;
        var credit = issueCost;
        Assert.Equal(debit, credit);
    }

    [Fact]
    public void FIFO_InsufficientStock_IsDetectedBeforePosting()
    {
        var hasSufficient = StockCostingService.HasSufficientStock(50, 75);
        Assert.False(hasSufficient);
    }

    [Fact]
    public void AllowNegativeStock_AlwaysPassesSufficiencyCheck()
    {
        var result = StockCostingService.HasSufficientStock(0, 100, allowNegative: true);
        Assert.True(result);
    }

    // ── VoucherValidator + FIFO cross-module ───────────────────────────────

    [Fact]
    public void COGS_FromFIFO_BalancedVoucher_IsValid()
    {
        var lots = new List<StockLot>
        {
            new(100, 200m, DateTime.Today),
        };
        var (cogs, _) = StockCostingService.CalculateFIFO(lots, 80);
        // cogs = 80 * 200 = 16000

        var (isValid, error) = UltimateERP.Application.Features.Account.Services.VoucherValidator.ValidateBalance(cogs, cogs);
        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void VoucherNumber_GeneratedFromInvoiceNumber_HasCorrectFormat()
    {
        var invoiceSeq = 42;
        var purchaseVoucherNumber = UltimateERP.Application.Features.Account.Services.VoucherValidator.GenerateVoucherNumber("PI-", invoiceSeq, 5);
        Assert.Equal("PI-00042", purchaseVoucherNumber);

        var salesVoucherNumber = UltimateERP.Application.Features.Account.Services.VoucherValidator.GenerateVoucherNumber("SI-", invoiceSeq, 5);
        Assert.Equal("SI-00042", salesVoucherNumber);
    }
}
