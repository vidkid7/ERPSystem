using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Setup;

public class UserDefinedField : BaseEntity
{
    public int EntityId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string FieldLabel { get; set; } = string.Empty;
    public UDFFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? SelectOptions { get; set; }
}
