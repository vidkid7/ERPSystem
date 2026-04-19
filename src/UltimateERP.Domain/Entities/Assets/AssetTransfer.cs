using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetTransfer : BaseEntity
{
    public int AssetId { get; set; }
    public AssetMaster Asset { get; set; } = null!;
    public int FromEmployeeId { get; set; }
    public Employee FromEmployee { get; set; } = null!;
    public int ToEmployeeId { get; set; }
    public Employee ToEmployee { get; set; } = null!;
    public DateTime TransferDate { get; set; }
    public string? Remarks { get; set; }
}
