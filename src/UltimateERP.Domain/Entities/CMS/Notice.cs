using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.CMS;

public class Notice : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
