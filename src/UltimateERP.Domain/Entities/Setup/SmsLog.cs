using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Setup;

public class SmsLog : BaseEntity
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsSent { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentDate { get; set; } = DateTime.UtcNow;
}
