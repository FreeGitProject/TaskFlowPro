using System.Net.Http.Json;
using TaskFlowPro.Shared.Dtos;

namespace TaskFlowPro.Client.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationsAsync();
        Task MarkAsReadAsync(int notificationId);
    }

    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<NotificationDto>>("api/notifications");
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var response = await _httpClient.PutAsync($"api/notifications/mark-as-read/{notificationId}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
