using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Finance;

public class VehicleDetail : BaseEntity
{
    public string? RegistrationNumber { get; set; }
    public string? ChassisNumber { get; set; }
    public string? EngineNumber { get; set; }
    public string? Model { get; set; }
    public string? Type { get; set; }
    public string? Color { get; set; }
    public int? ManufacturingYear { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal PurchaseValue { get; set; }
}
