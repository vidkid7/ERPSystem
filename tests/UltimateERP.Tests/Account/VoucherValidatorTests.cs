using UltimateERP.Application.Features.Account.Services;

namespace UltimateERP.Tests.Account;

public class VoucherValidatorTests
{
    [Fact]
    public void ValidateBalance_Balanced_ReturnsValid()
    {
        var (isValid, error) = VoucherValidator.ValidateBalance(10000m, 10000m);
        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void ValidateBalance_SlightlyOff_ButWithinTolerance_ReturnsValid()
    {
        var (isValid, error) = VoucherValidator.ValidateBalance(10000.005m, 10000.01m);
        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void ValidateBalance_Unbalanced_ReturnsError()
    {
        var (isValid, error) = VoucherValidator.ValidateBalance(10000m, 9000m);
        Assert.False(isValid);
        Assert.Contains("not balanced", error!);
    }

    [Fact]
    public void ValidateBalance_ZeroAmounts_ReturnsError()
    {
        var (isValid, error) = VoucherValidator.ValidateBalance(0, 0);
        Assert.False(isValid);
        Assert.Contains("at least one", error!);
    }

    [Fact]
    public void GenerateVoucherNumber_ProducesCorrectFormat()
    {
        var number = VoucherValidator.GenerateVoucherNumber("JV-", 42, 6);
        Assert.Equal("JV-000042", number);
    }

    [Fact]
    public void GenerateVoucherNumber_WithDifferentWidth()
    {
        var number = VoucherValidator.GenerateVoucherNumber("PV-", 1, 4);
        Assert.Equal("PV-0001", number);
    }
}
