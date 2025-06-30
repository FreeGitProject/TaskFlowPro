using TaskFlowPro.Api.Dtos;

namespace TaskFlowPro.Api.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(CreateTaskDto taskDto, string userId);
        Task UpdateTaskAsync(UpdateTaskDto taskDto, string userId);
        Task DeleteTaskAsync(int id, string userId);
        Task<List<TaskDto>> GetTasksAssignedToUserAsync(string userId);
    }
}
