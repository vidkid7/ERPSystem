using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.CMS;

public class Slider : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? LinkURL { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
