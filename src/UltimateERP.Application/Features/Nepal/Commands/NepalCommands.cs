using MediatR;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Nepal.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Nepal.Commands;

// Submit Sales Data to IRD
public record SubmitSalesToIRDCommand(IRDSalesDataDto SalesData) : IRequest<ApiResponse<IRDSubmissionResultDto>>;

public class SubmitSalesToIRDHandler : IRequestHandler<SubmitSalesToIRDCommand, ApiResponse<IRDSubmissionResultDto>>
{
    private readonly IIRDApiClient _irdClient;
    private readonly ILogger<SubmitSalesToIRDHandler> _logger;

    public SubmitSalesToIRDHandler(IIRDApiClient irdClient, ILogger<SubmitSalesToIRDHandler> logger)
    {
        _irdClient = irdClient;
        _logger = logger;
    }

    public async Task<ApiResponse<IRDSubmissionResultDto>> Handle(SubmitSalesToIRDCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Submitting sales invoice {InvoiceNo} to IRD", request.SalesData.InvoiceNo);

        var result = await _irdClient.SubmitSalesData(request.SalesData, ct);
        return result.IsSuccess
            ? ApiResponse<IRDSubmissionResultDto>.Success(result, "Sales data submitted to IRD successfully")
            : ApiResponse<IRDSubmissionResultDto>.Failure($"IRD submission failed: {result.Message}");
    }
}

// Submit Purchase Data to IRD
public record SubmitPurchaseToIRDCommand(IRDPurchaseDataDto PurchaseData) : IRequest<ApiResponse<IRDSubmissionResultDto>>;

public class SubmitPurchaseToIRDHandler : IRequestHandler<SubmitPurchaseToIRDCommand, ApiResponse<IRDSubmissionResultDto>>
{
    private readonly IIRDApiClient _irdClient;
    private readonly ILogger<SubmitPurchaseToIRDHandler> _logger;

    public SubmitPurchaseToIRDHandler(IIRDApiClient irdClient, ILogger<SubmitPurchaseToIRDHandler> logger)
    {
        _irdClient = irdClient;
        _logger = logger;
    }

    public async Task<ApiResponse<IRDSubmissionResultDto>> Handle(SubmitPurchaseToIRDCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Submitting purchase invoice {InvoiceNo} to IRD", request.PurchaseData.InvoiceNo);

        var result = await _irdClient.SubmitPurchaseData(request.PurchaseData, ct);
        return result.IsSuccess
            ? ApiResponse<IRDSubmissionResultDto>.Success(result, "Purchase data submitted to IRD successfully")
            : ApiResponse<IRDSubmissionResultDto>.Failure($"IRD submission failed: {result.Message}");
    }
}
