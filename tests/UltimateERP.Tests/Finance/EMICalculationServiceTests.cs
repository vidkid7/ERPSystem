using UltimateERP.Application.Features.Finance.Services;

namespace UltimateERP.Tests.Finance;

public class EMICalculationServiceTests
{
    [Fact]
    public void CalculateEMI_StandardLoan_ReturnsCorrectEMI()
    {
        var emi = EMICalculationService.CalculateEMI(100000m, 12m, 12);
        Assert.Equal(8884.88m, emi);
    }

    [Fact]
    public void CalculateEMI_ZeroRate_ReturnsFlatDivision()
    {
        var emi = EMICalculationService.CalculateEMI(100000m, 0m, 12);
        Assert.Equal(8333.33m, emi);
    }

    [Fact]
    public void GenerateSchedule_HasCorrectNumberOfItems()
    {
        var schedule = EMICalculationService.GenerateSchedule(100000m, 12m, 12, DateTime.Today);
        Assert.Equal(12, schedule.Count);
    }

    [Fact]
    public void GenerateSchedule_LastItemHasZeroBalance()
    {
        var schedule = EMICalculationService.GenerateSchedule(100000m, 12m, 12, DateTime.Today);
        Assert.Equal(0m, schedule.Last().OutstandingBalance);
    }

    [Fact]
    public void CalculateRebate_ReturnsCorrectAmount()
    {
        var rebate = EMICalculationService.CalculateRebate(1000m, 5m);
        Assert.Equal(50.00m, rebate);
    }

    [Fact]
    public void CalculatePenalty_ReturnsCorrectAmount()
    {
        var penalty = EMICalculationService.CalculatePenalty(5000m, 2m);
        Assert.Equal(100.00m, penalty);
    }
}
