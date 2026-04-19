using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Service;

public class DeviceType : BaseEntity { }

public class DeviceModel : BaseEntity
{
    public int DeviceTypeId { get; set; }
    public DeviceType DeviceType { get; set; } = null!;
}
