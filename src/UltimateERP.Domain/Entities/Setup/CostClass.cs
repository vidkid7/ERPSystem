using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Setup;

public class CostClass : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? StartMiti { get; set; }
    public string? EndMiti { get; set; }
    public bool IsDefault { get; set; }
    public bool BlockTransaction { get; set; }
}
