namespace UltimateERP.Application.Interfaces;

public interface IExcelService
{
    Task<byte[]> ExportToExcelAsync<T>(List<T> data, string sheetName);
    Task<List<T>> ImportFromExcelAsync<T>(Stream fileStream) where T : new();
}
