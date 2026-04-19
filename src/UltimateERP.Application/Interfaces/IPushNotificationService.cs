namespace UltimateERP.Application.Interfaces;

public interface IPushNotificationService
{
    Task SendNotificationAsync(string userId, string title, string message);
    Task SendBulkNotificationAsync(List<string> userIds, string title, string message);
}
