using System.Net.Http.Json;
using TaskFlowPro.Shared.Dtos;

namespace TaskFlowPro.Client.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(CreateTaskDto task);
        Task UpdateTaskAsync(UpdateTaskDto task);
        Task DeleteTaskAsync(int id);
        Task<List<TaskDto>> GetTasksAssignedToMeAsync();
    }

    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskDto>>("api/tasks");
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskDto>($"api/tasks/{id}");
        }

        public async Task CreateTaskAsync(CreateTaskDto task)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTaskAsync(UpdateTaskDto task)
        {
            var response = await _httpClient.PutAsJsonAsync("api/tasks", task);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<TaskDto>> GetTasksAssignedToMeAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskDto>>("api/tasks/assigned-to-me");
        }
    }
}
