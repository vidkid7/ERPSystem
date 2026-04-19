namespace UltimateERP.Application.Features.Reporting.DTOs;

public class ReportColumnDto
{
    public string FieldName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int Width { get; set; } = 100;
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
}

public class ReportDefinitionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public List<ReportColumnDto> Columns { get; set; } = new();
    public string? Filters { get; set; }
    public string? GroupBy { get; set; }
    public string? SortBy { get; set; }
    public bool IsShared { get; set; }
    public int? CreatedById { get; set; }
}

public class CreateReportDefinitionDto
{
    public string Name { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public List<ReportColumnDto> Columns { get; set; } = new();
    public string? Filters { get; set; }
    public string? GroupBy { get; set; }
    public string? SortBy { get; set; }
    public bool IsShared { get; set; }
}

public class UpdateReportDefinitionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public List<ReportColumnDto> Columns { get; set; } = new();
    public string? Filters { get; set; }
    public string? GroupBy { get; set; }
    public string? SortBy { get; set; }
    public bool IsShared { get; set; }
}

public class CustomReportResultDto
{
    public string ReportName { get; set; } = string.Empty;
    public List<string> ColumnHeaders { get; set; } = new();
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
    public int TotalRows { get; set; }
}
