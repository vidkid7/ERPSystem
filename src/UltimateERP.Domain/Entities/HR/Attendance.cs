using UltimateERP.Domain.Common;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Domain.Entities.HR;

public class Attendance : BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime AttendanceDate { get; set; }
    public string? AttendanceDateBS { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public decimal WorkingHours { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
}
