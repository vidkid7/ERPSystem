using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Setup;

public class DocumentType : BaseEntity
{
    public string? Description { get; set; }
    public string? Prefix { get; set; }
    public string? Module { get; set; }
}
