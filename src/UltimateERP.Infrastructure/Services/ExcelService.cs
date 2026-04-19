using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.Services;

public class ExcelService : IExcelService
{
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(ILogger<ExcelService> logger) => _logger = logger;

    public Task<byte[]> ExportToExcelAsync<T>(List<T> data, string sheetName)
    {
        _logger.LogInformation("Exporting {Count} records to CSV: {SheetName}", data.Count, sheetName);

        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Header row
        sb.AppendLine(string.Join(",", properties.Select(p => EscapeCsvField(p.Name))));

        // Data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var val = p.GetValue(item);
                return EscapeCsvField(val?.ToString() ?? string.Empty);
            });
            sb.AppendLine(string.Join(",", values));
        }

        return Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
    }

    public Task<List<T>> ImportFromExcelAsync<T>(Stream fileStream) where T : new()
    {
        _logger.LogInformation("Importing records from CSV");

        using var reader = new StreamReader(fileStream);
        var lines = new List<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null) lines.Add(line);
        }

        if (lines.Count < 2)
            return Task.FromResult(new List<T>());

        var headers = ParseCsvLine(lines[0]);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var result = new List<T>();

        for (int i = 1; i < lines.Count; i++)
        {
            var values = ParseCsvLine(lines[i]);
            var item = new T();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var prop = properties.FirstOrDefault(p =>
                    string.Equals(p.Name, headers[j].Trim(), StringComparison.OrdinalIgnoreCase));

                if (prop != null && prop.CanWrite)
                {
                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        if (!string.IsNullOrWhiteSpace(values[j]))
                        {
                            var converted = Convert.ChangeType(values[j].Trim(), targetType);
                            prop.SetValue(item, converted);
                        }
                    }
                    catch
                    {
                        // Skip fields that can't be converted
                    }
                }
            }

            result.Add(item);
        }

        _logger.LogInformation("Imported {Count} records from CSV", result.Count);
        return Task.FromResult(result);
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
            return $"\"{field.Replace("\"", "\"\"")}\"";
        return field;
    }

    private static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        bool inQuotes = false;
        var current = new StringBuilder();

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        fields.Add(current.ToString());
        return fields.ToArray();
    }
}
