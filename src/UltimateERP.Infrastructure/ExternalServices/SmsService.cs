using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.ExternalServices;

public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SmsService> _logger;
    private readonly string _providerUrl;
    private readonly string _apiKey;

    public SmsService(HttpClient httpClient, ILogger<SmsService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _providerUrl = configuration["SmsGateway:ProviderUrl"] ?? "https://sms-api.example.com";
        _apiKey = configuration["SmsGateway:ApiKey"] ?? string.Empty;
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            _logger.LogInformation("Sending SMS to {PhoneNumber}", phoneNumber);

            var payload = new { to = phoneNumber, message, apiKey = _apiKey };
            var response = await _httpClient.PostAsJsonAsync($"{_providerUrl}/send", payload);

            var success = response.IsSuccessStatusCode;
            _logger.LogInformation("SMS to {PhoneNumber}: {Status}", phoneNumber, success ? "Sent" : "Failed");
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendBulkSmsAsync(List<string> phoneNumbers, string message)
    {
        try
        {
            _logger.LogInformation("Sending bulk SMS to {Count} recipients", phoneNumbers.Count);

            var payload = new { recipients = phoneNumbers, message, apiKey = _apiKey };
            var response = await _httpClient.PostAsJsonAsync($"{_providerUrl}/send-bulk", payload);

            var success = response.IsSuccessStatusCode;
            _logger.LogInformation("Bulk SMS to {Count} recipients: {Status}", phoneNumbers.Count, success ? "Sent" : "Failed");
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send bulk SMS to {Count} recipients", phoneNumbers.Count);
            return false;
        }
    }
}
