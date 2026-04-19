using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.HMS;

public class Bed : BaseEntity
{
    public string BedNumber { get; set; } = string.Empty;
    public int? WardId { get; set; }
    public int? RoomId { get; set; }
    public int? FloorId { get; set; }
    public int? BuildingTypeId { get; set; }
    public string? BedType { get; set; }
    public BedStatus Status { get; set; }
}
