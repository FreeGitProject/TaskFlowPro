using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskFlowPro.Api.Data;
using TaskFlowPro.Api.Dtos;
using TaskFlowPro.Api.Hubs;
using TaskFlowPro.Api.Models;

using TaskStatus = TaskFlowPro.Api.Models.TaskStatus;

namespace TaskFlowPro.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskService(AppDbContext context, IHubContext<TaskHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async System.Threading.Tasks.Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                throw new KeyNotFoundException("Task not found");

            return MapToDto(task);
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(CreateTaskDto taskDto, string userId)
        {
            var task = new Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = Enum.Parse<TaskStatus>(taskDto.Status),
                Priority = Enum.Parse<TaskPriority>(taskDto.Priority),
                CreatedById = userId,
                AssignedToId = taskDto.AssignedToId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Send real-time update
            await _hubContext.Clients.Group($"task-{task.Id}").SendAsync("TaskUpdated", $"New task created: {task.Title}");
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(UpdateTaskDto taskDto, string userId)
        {
            var task = await _context.Tasks.FindAsync(taskDto.Id);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Status = Enum.Parse<TaskStatus>(taskDto.Status);
            task.Priority = Enum.Parse<TaskPriority>(taskDto.Priority);
            task.AssignedToId = taskDto.AssignedToId;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Send real-time update
            await _hubContext.Clients.Group($"task-{task.Id}").SendAsync("TaskUpdated", $"Task updated: {task.Title}");
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id, string userId)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            // Send real-time update
            await _hubContext.Clients.Group($"task-{id}").SendAsync("TaskUpdated", $"Task deleted: {task.Title}");
        }

        public async System.Threading.Tasks.Task<List<TaskDto>> GetTasksAssignedToUserAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedToId == userId)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        private static TaskDto MapToDto(Models.Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                CreatedById = task.CreatedById,
                CreatedBy = $"{task.CreatedBy?.FirstName} {task.CreatedBy?.LastName}",
                AssignedToId = task.AssignedToId,
                AssignedTo = task.AssignedTo != null ? $"{task.AssignedTo.FirstName} {task.AssignedTo.LastName}" : null
            };
        }
    }
}
