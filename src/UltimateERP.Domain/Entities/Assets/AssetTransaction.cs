using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Assets;

public class AssetTransaction : BaseEntity
{
    public int AssetId { get; set; }
    public Asset? Asset { get; set; }
    public AssetTransactionType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public int? FromEmployeeId { get; set; }
    public Employee? FromEmployee { get; set; }
    public int? ToEmployeeId { get; set; }
    public Employee? ToEmployee { get; set; }
    public string? Remarks { get; set; }
    public decimal Amount { get; set; }
    public string? DocumentNo { get; set; }
}
