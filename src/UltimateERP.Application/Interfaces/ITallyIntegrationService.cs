using UltimateERP.Application.Features.Integration.DTOs;

namespace UltimateERP.Application.Interfaces;

public interface ITallyIntegrationService
{
    Task<ImportResultDto> ImportFromTallyXmlAsync(Stream xmlStream);
    Task<byte[]> ExportToTallyXmlAsync(TallyExportRequestDto request);
}
