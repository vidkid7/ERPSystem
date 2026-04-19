using UltimateERP.Domain.Interfaces;

namespace UltimateERP.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateOnly TodayAD => DateOnly.FromDateTime(DateTime.UtcNow);

    // Placeholder — full BS conversion will be implemented in Task 20 (Nepal Compliance)
    public string TodayBS => ConvertADtoBS(TodayAD);

    private static string ConvertADtoBS(DateOnly adDate)
    {
        // Simplified placeholder; real implementation uses lookup tables
        return adDate.ToString("yyyy-MM-dd") + " (BS-TBD)";
    }
}
