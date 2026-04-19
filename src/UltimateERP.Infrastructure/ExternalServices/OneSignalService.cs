using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Infrastructure.ExternalServices;

public class OneSignalService : IPushNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OneSignalService> _logger;
    private readonly string _appId;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public OneSignalService(HttpClient httpClient, ILogger<OneSignalService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _appId = configuration["OneSignal:AppId"] ?? string.Empty;
        _apiKey = configuration["OneSignal:ApiKey"] ?? string.Empty;
        _baseUrl = configuration["OneSignal:BaseUrl"] ?? "https://onesignal.com/api/v1";
    }

    public async Task SendNotificationAsync(string userId, string title, string message)
    {
        try
        {
            _logger.LogInformation("Sending push notification to user {UserId}: {Title}", userId, title);

            var payload = new
            {
                app_id = _appId,
                include_external_user_ids = new[] { userId },
                headings = new { en = title },
                contents = new { en = message }
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _apiKey);

            await _httpClient.PostAsJsonAsync($"{_baseUrl}/notifications", payload);
            _logger.LogInformation("Push notification sent to user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send push notification to user {UserId}", userId);
        }
    }

    public async Task SendBulkNotificationAsync(List<string> userIds, string title, string message)
    {
        try
        {
            _logger.LogInformation("Sending bulk push notification to {Count} users: {Title}", userIds.Count, title);

            var payload = new
            {
                app_id = _appId,
                include_external_user_ids = userIds,
                headings = new { en = title },
                contents = new { en = message }
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _apiKey);

            await _httpClient.PostAsJsonAsync($"{_baseUrl}/notifications", payload);
            _logger.LogInformation("Bulk push notification sent to {Count} users", userIds.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send bulk push notification to {Count} users", userIds.Count);
        }
    }
}
