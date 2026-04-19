using UltimateERP.Application.Services;

namespace UltimateERP.Tests.Nepal;

public class BikramSambatServiceTests
{
    [Fact]
    public void ADToBS_KnownDate_ReturnsCorrectBSDate()
    {
        // 1 Baisakh 2000 BS = 14 April 1943 AD (reference date)
        var result = BikramSambatService.ADToBS(new DateTime(1943, 4, 14));
        Assert.Equal("2000/01/01", result);
    }

    [Fact]
    public void ADToBS_RecentDate_ReturnsValidFormat()
    {
        // 1 Jan 2024 AD should be in the BS range and formatted correctly
        var result = BikramSambatService.ADToBS(new DateTime(2024, 1, 1));
        Assert.Matches(@"^\d{4}/\d{2}/\d{2}$", result);
        Assert.StartsWith("2080/", result);
    }

    [Fact]
    public void ADToBS_SpecificKnownDate_2077()
    {
        // 14 April 2020 AD = 1 Baisakh 2077 BS
        var result = BikramSambatService.ADToBS(new DateTime(2020, 4, 14));
        Assert.StartsWith("2077/01/", result);
    }

    [Fact]
    public void BSToAD_KnownDate_ReturnsCorrectADDate()
    {
        // 1 Baisakh 2000 BS = 14 April 1943 AD
        var result = BikramSambatService.BSToAD("2000/01/01");
        Assert.Equal(new DateTime(1943, 4, 14), result);
    }

    [Fact]
    public void ADToBS_BSToAD_Roundtrip()
    {
        var originalDate = new DateTime(2024, 6, 15);
        var bsDate = BikramSambatService.ADToBS(originalDate);
        var roundtripped = BikramSambatService.BSToAD(bsDate);
        Assert.Equal(originalDate, roundtripped);
    }

    [Fact]
    public void ADToBS_BSToAD_Roundtrip_MultipleYears()
    {
        // Test multiple dates across different years
        var dates = new[]
        {
            new DateTime(1950, 1, 1),
            new DateTime(1975, 6, 15),
            new DateTime(2000, 12, 31),
            new DateTime(2010, 3, 14),
            new DateTime(2020, 7, 20),
        };

        foreach (var date in dates)
        {
            var bs = BikramSambatService.ADToBS(date);
            var ad = BikramSambatService.BSToAD(bs);
            Assert.Equal(date, ad);
        }
    }

    [Fact]
    public void BSToAD_InvalidFormat_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => BikramSambatService.BSToAD("2080-01-01"));
    }

    [Fact]
    public void BSToAD_InvalidMonth_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BikramSambatService.BSToAD("2080/13/01"));
    }

    [Fact]
    public void BSToAD_InvalidDay_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BikramSambatService.BSToAD("2080/01/35"));
    }

    [Fact]
    public void ADToBS_DateBeforeRange_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BikramSambatService.ADToBS(new DateTime(1940, 1, 1)));
    }

    [Fact]
    public void GetBSMonthName_ValidMonths_ReturnsCorrectNames()
    {
        Assert.Equal("Baisakh", BikramSambatService.GetBSMonthName(1));
        Assert.Equal("Jestha", BikramSambatService.GetBSMonthName(2));
        Assert.Equal("Ashadh", BikramSambatService.GetBSMonthName(3));
        Assert.Equal("Shrawan", BikramSambatService.GetBSMonthName(4));
        Assert.Equal("Bhadra", BikramSambatService.GetBSMonthName(5));
        Assert.Equal("Ashwin", BikramSambatService.GetBSMonthName(6));
        Assert.Equal("Kartik", BikramSambatService.GetBSMonthName(7));
        Assert.Equal("Mangsir", BikramSambatService.GetBSMonthName(8));
        Assert.Equal("Poush", BikramSambatService.GetBSMonthName(9));
        Assert.Equal("Magh", BikramSambatService.GetBSMonthName(10));
        Assert.Equal("Falgun", BikramSambatService.GetBSMonthName(11));
        Assert.Equal("Chaitra", BikramSambatService.GetBSMonthName(12));
    }

    [Fact]
    public void GetBSMonthName_InvalidMonth_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BikramSambatService.GetBSMonthName(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => BikramSambatService.GetBSMonthName(13));
    }
}
