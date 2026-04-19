using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Features.Nepal.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.ExternalServices;

/// <summary>
/// HttpClient-based implementation for Nepal IRD CBMS API integration.
/// </summary>
public class IRDApiClient : IIRDApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IRDApiClient> _logger;

    public IRDApiClient(HttpClient httpClient, ILogger<IRDApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IRDSubmissionResultDto> SubmitSalesData(IRDSalesDataDto salesData, CancellationToken ct = default)
    {
        _logger.LogInformation("Submitting sales data to IRD CBMS API. Invoice: {InvoiceNo}", salesData.InvoiceNo);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/billreturn", salesData, ct);
            var result = await HandleResponse(response, ct);
            _logger.LogInformation("IRD sales submission result: {Success}, ID: {SubmissionId}", result.IsSuccess, result.SubmissionId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit sales data to IRD for invoice {InvoiceNo}", salesData.InvoiceNo);
            return new IRDSubmissionResultDto
            {
                IsSuccess = false,
                Message = $"IRD API call failed: {ex.Message}",
                SubmittedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<IRDSubmissionResultDto> SubmitPurchaseData(IRDPurchaseDataDto purchaseData, CancellationToken ct = default)
    {
        _logger.LogInformation("Submitting purchase data to IRD CBMS API. Invoice: {InvoiceNo}", purchaseData.InvoiceNo);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/purchasereturn", purchaseData, ct);
            var result = await HandleResponse(response, ct);
            _logger.LogInformation("IRD purchase submission result: {Success}, ID: {SubmissionId}", result.IsSuccess, result.SubmissionId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit purchase data to IRD for invoice {InvoiceNo}", purchaseData.InvoiceNo);
            return new IRDSubmissionResultDto
            {
                IsSuccess = false,
                Message = $"IRD API call failed: {ex.Message}",
                SubmittedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<IRDSubmissionResultDto> GetSubmissionStatus(string submissionId, CancellationToken ct = default)
    {
        _logger.LogInformation("Checking IRD submission status for ID: {SubmissionId}", submissionId);

        try
        {
            var response = await _httpClient.GetAsync($"api/billstatus/{submissionId}", ct);
            var result = await HandleResponse(response, ct);
            _logger.LogInformation("IRD status check result: {Success}, Message: {Message}", result.IsSuccess, result.Message);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check IRD submission status for ID {SubmissionId}", submissionId);
            return new IRDSubmissionResultDto
            {
                IsSuccess = false,
                SubmissionId = submissionId,
                Message = $"IRD API call failed: {ex.Message}",
                SubmittedAt = DateTime.UtcNow
            };
        }
    }

    private static async Task<IRDSubmissionResultDto> HandleResponse(HttpResponseMessage response, CancellationToken ct)
    {
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.IsSuccessStatusCode)
        {
            return new IRDSubmissionResultDto
            {
                IsSuccess = true,
                SubmissionId = TryExtractField(body, "submission_id"),
                Message = TryExtractField(body, "message") ?? "Success",
                SubmittedAt = DateTime.UtcNow
            };
        }

        return new IRDSubmissionResultDto
        {
            IsSuccess = false,
            ErrorCode = ((int)response.StatusCode).ToString(),
            Message = TryExtractField(body, "message") ?? $"IRD API returned {response.StatusCode}",
            SubmittedAt = DateTime.UtcNow
        };
    }

    private static string? TryExtractField(string json, string fieldName)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty(fieldName, out var value))
                return value.ToString();
        }
        catch
        {
            // Not valid JSON, ignore
        }
        return null;
    }
}
