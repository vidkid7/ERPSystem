using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Setup;

public class EntityNumbering : BaseEntity
{
    public int EntityId { get; set; }
    public NumberingMethod NumberingMethod { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int NumericalPartWidth { get; set; }
    public int StartNumber { get; set; }
    public int CurrentNumber { get; set; }
}
