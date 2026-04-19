using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Setup;

public class Branch : BaseEntity
{
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsHeadOffice { get; set; }
}
