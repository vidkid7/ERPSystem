using MediatR;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Integration.Commands;

public record ImportFromTallyCommand(Stream XmlStream) : IRequest<ApiResponse<ImportResultDto>>;

public class ImportFromTallyHandler : IRequestHandler<ImportFromTallyCommand, ApiResponse<ImportResultDto>>
{
    private readonly ITallyIntegrationService _tallyService;

    public ImportFromTallyHandler(ITallyIntegrationService tallyService) => _tallyService = tallyService;

    public async Task<ApiResponse<ImportResultDto>> Handle(ImportFromTallyCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _tallyService.ImportFromTallyXmlAsync(request.XmlStream);
            return ApiResponse<ImportResultDto>.Success(result, $"Tally import completed. {result.SuccessCount} of {result.TotalRows} records imported");
        }
        catch (Exception ex)
        {
            return ApiResponse<ImportResultDto>.Failure($"Tally import failed: {ex.Message}");
        }
    }
}

public record ExportToTallyCommand(TallyExportRequestDto Request) : IRequest<ApiResponse<byte[]>>;

public class ExportToTallyHandler : IRequestHandler<ExportToTallyCommand, ApiResponse<byte[]>>
{
    private readonly ITallyIntegrationService _tallyService;

    public ExportToTallyHandler(ITallyIntegrationService tallyService) => _tallyService = tallyService;

    public async Task<ApiResponse<byte[]>> Handle(ExportToTallyCommand request, CancellationToken ct)
    {
        try
        {
            var bytes = await _tallyService.ExportToTallyXmlAsync(request.Request);
            return ApiResponse<byte[]>.Success(bytes, "Tally export completed");
        }
        catch (Exception ex)
        {
            return ApiResponse<byte[]>.Failure($"Tally export failed: {ex.Message}");
        }
    }
}
