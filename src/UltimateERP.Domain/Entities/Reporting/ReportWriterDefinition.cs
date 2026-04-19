using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Domain.Entities.Reporting;

public class ReportWriterDefinition : BaseEntity
{
    public string ReportName { get; set; } = string.Empty;
    public string? ReportTitle { get; set; }
    public int? ModuleId { get; set; }
    public string? EntityQuery { get; set; }
    public string? ReportLayout { get; set; }
    public bool IsSharedWithAll { get; set; }
    public int? CreatedById { get; set; }
    public User? CreatedByUser { get; set; }
}
