using UltimateERP.Domain.Common;
using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Domain.Entities.Reporting;

public class DynamicAIDashboard : BaseEntity
{
    public string DashboardName { get; set; } = string.Empty;
    public string? WidgetDefinitions { get; set; }
    public bool IsDefault { get; set; }
    public int? CreatedById { get; set; }
    public User? CreatedByUser { get; set; }
}

public class CustomDashboard : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string DashboardName { get; set; } = string.Empty;
    public string? DashboardConfig { get; set; }
    public bool IsDefault { get; set; }

    public ICollection<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
}

public class DashboardWidget : BaseEntity
{
    public int CustomDashboardId { get; set; }
    public CustomDashboard CustomDashboard { get; set; } = null!;
    public string? WidgetType { get; set; }
    public string? WidgetConfig { get; set; }
    public int Position { get; set; }
    public string? Size { get; set; }
}
