namespace UltimateERP.Domain.Interfaces;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateOnly TodayAD { get; }
    string TodayBS { get; }
}
