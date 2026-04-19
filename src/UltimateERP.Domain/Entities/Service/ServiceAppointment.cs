using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.Service;

public class ServiceAppointment : BaseEntity
{
    public string AppointmentNumber { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string? AppointmentDateBS { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? DeviceModelId { get; set; }
    public DeviceModel? DeviceModel { get; set; }
    public int? ServiceTypeId { get; set; }
    public int? AssignedToId { get; set; }
    public Employee? AssignedTo { get; set; }
    public ServiceAppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
}
