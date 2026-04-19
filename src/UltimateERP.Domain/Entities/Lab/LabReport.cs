using UltimateERP.Domain.Common;

namespace UltimateERP.Domain.Entities.Lab;

public class LabReport : BaseEntity
{
    public int SampleCollectionId { get; set; }
    public SampleCollection SampleCollection { get; set; } = null!;
    public DateTime ReportDate { get; set; }
    public string? ReportDateBS { get; set; }
    public int? TemplateId { get; set; }
    public string? ReportData { get; set; }
    public int? GeneratedBy { get; set; }
}
