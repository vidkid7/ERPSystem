using UltimateERP.Application.Features.Nepal.DTOs;

namespace UltimateERP.Application.Interfaces;

/// <summary>
/// Client interface for Nepal Inland Revenue Department (IRD) CBMS API integration.
/// </summary>
public interface IIRDApiClient
{
    Task<IRDSubmissionResultDto> SubmitSalesData(IRDSalesDataDto salesData, CancellationToken ct = default);
    Task<IRDSubmissionResultDto> SubmitPurchaseData(IRDPurchaseDataDto purchaseData, CancellationToken ct = default);
    Task<IRDSubmissionResultDto> GetSubmissionStatus(string submissionId, CancellationToken ct = default);
}
