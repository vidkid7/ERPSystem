namespace UltimateERP.Application.Services;

/// <summary>
/// Provides static conversion methods between AD (Gregorian) and BS (Bikram Sambat) calendars.
/// Uses lookup tables for BS years 2000-2090.
/// </summary>
public static class BikramSambatService
{
    private static readonly DateTime ReferenceAD = new(1943, 4, 14); // 1 Baisakh 2000 BS
    private const int ReferenceYearBS = 2000;

    private static readonly string[] MonthNames =
    {
        "Baisakh", "Jestha", "Ashadh", "Shrawan", "Bhadra", "Ashwin",
        "Kartik", "Mangsir", "Poush", "Magh", "Falgun", "Chaitra"
    };

    // Days per month for BS years 2000-2090 (index 0 = year 2000)
    private static readonly int[][] BsMonthDays =
    {
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2000
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2001
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2002
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2003
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2004
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2005
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2006
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2007
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31 }, // 2008
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2009
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2010
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2011
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 }, // 2012
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2013
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2014
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2015
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 }, // 2016
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2017
        new[] { 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2018
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2019
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2020
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2021
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2022
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2023
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2024
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2025
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2026
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2027
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2028
        new[] { 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30 }, // 2029
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2030
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2031
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2032
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2033
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2034
        new[] { 30, 32, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31 }, // 2035
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2036
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2037
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2038
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 }, // 2039
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2040
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2041
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2042
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 }, // 2043
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2044
        new[] { 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2045
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2046
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2047
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2048
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2049
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2050
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2051
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2052
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2053
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2054
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2055
        new[] { 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30 }, // 2056
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2057
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2058
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2059
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2060
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2061
        new[] { 30, 32, 31, 32, 31, 31, 29, 30, 29, 30, 29, 31 }, // 2062
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2063
        new[] { 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2064
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2065
        new[] { 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 }, // 2066
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2067
        new[] { 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 }, // 2068
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 }, // 2069
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2070
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2071
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2072
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2073
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2074
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2075
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2076
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 }, // 2077
        new[] { 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2078
        new[] { 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 }, // 2079
        new[] { 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 }, // 2080
        new[] { 31, 31, 32, 32, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2081
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2082
        new[] { 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2083
        new[] { 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2084
        new[] { 31, 32, 31, 32, 30, 31, 30, 30, 29, 30, 30, 30 }, // 2085
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2086
        new[] { 31, 31, 32, 31, 31, 31, 30, 30, 29, 30, 30, 30 }, // 2087
        new[] { 30, 31, 32, 32, 30, 31, 30, 30, 29, 30, 30, 30 }, // 2088
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2089
        new[] { 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 }, // 2090
    };

    public static string GetBSMonthName(int month)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        return MonthNames[month - 1];
    }

    /// <summary>
    /// Converts an AD (Gregorian) date to BS (Bikram Sambat) date string in YYYY/MM/DD format.
    /// </summary>
    public static string ADToBS(DateTime adDate)
    {
        if (adDate < ReferenceAD)
            throw new ArgumentOutOfRangeException(nameof(adDate), "Date must be on or after 2000/01/01 BS (1943-04-14 AD).");

        int totalDays = (int)(adDate.Date - ReferenceAD).TotalDays;

        int bsYear = ReferenceYearBS;
        int bsMonth = 1;
        int bsDay = 1;

        while (totalDays > 0)
        {
            int idx = bsYear - ReferenceYearBS;
            if (idx < 0 || idx >= BsMonthDays.Length)
                throw new ArgumentOutOfRangeException(nameof(adDate), "Date is outside the supported BS range (2000-2090 BS).");

            int daysInMonth = BsMonthDays[idx][bsMonth - 1];

            if (totalDays < daysInMonth - bsDay + 1)
            {
                bsDay += totalDays;
                totalDays = 0;
            }
            else
            {
                totalDays -= (daysInMonth - bsDay + 1);
                bsMonth++;
                bsDay = 1;

                if (bsMonth > 12)
                {
                    bsMonth = 1;
                    bsYear++;
                }
            }
        }

        return $"{bsYear:D4}/{bsMonth:D2}/{bsDay:D2}";
    }

    /// <summary>
    /// Converts a BS date string (YYYY/MM/DD) to AD (Gregorian) DateTime.
    /// </summary>
    public static DateTime BSToAD(string bsDate)
    {
        var parts = bsDate.Split('/');
        if (parts.Length != 3)
            throw new FormatException("BS date must be in YYYY/MM/DD format.");

        int bsYear = int.Parse(parts[0]);
        int bsMonth = int.Parse(parts[1]);
        int bsDay = int.Parse(parts[2]);

        if (bsYear < ReferenceYearBS || bsYear - ReferenceYearBS >= BsMonthDays.Length)
            throw new ArgumentOutOfRangeException(nameof(bsDate), "BS year is outside the supported range (2000-2090).");
        if (bsMonth < 1 || bsMonth > 12)
            throw new ArgumentOutOfRangeException(nameof(bsDate), "BS month must be between 1 and 12.");

        int idx = bsYear - ReferenceYearBS;
        int daysInMonth = BsMonthDays[idx][bsMonth - 1];
        if (bsDay < 1 || bsDay > daysInMonth)
            throw new ArgumentOutOfRangeException(nameof(bsDate), $"BS day must be between 1 and {daysInMonth} for {GetBSMonthName(bsMonth)} {bsYear}.");

        int totalDays = 0;

        // Add days for full years from reference to bsYear
        for (int y = ReferenceYearBS; y < bsYear; y++)
        {
            int yIdx = y - ReferenceYearBS;
            for (int m = 0; m < 12; m++)
                totalDays += BsMonthDays[yIdx][m];
        }

        // Add days for full months in the target year
        for (int m = 0; m < bsMonth - 1; m++)
            totalDays += BsMonthDays[idx][m];

        // Add remaining days
        totalDays += bsDay - 1;

        return ReferenceAD.AddDays(totalDays);
    }

    internal static int GetDaysInBsYear(int bsYear)
    {
        int idx = bsYear - ReferenceYearBS;
        if (idx < 0 || idx >= BsMonthDays.Length)
            throw new ArgumentOutOfRangeException(nameof(bsYear));
        return BsMonthDays[idx].Sum();
    }
}
