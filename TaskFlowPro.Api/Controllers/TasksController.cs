using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlowPro.Api.Dtos;
using TaskFlowPro.Api.Models;
using TaskFlowPro.Api.Services;

namespace TaskFlowPro.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(CreateTaskDto taskDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskService.CreateTaskAsync(taskDto, userId);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTask(UpdateTaskDto taskDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskService.UpdateTaskAsync(taskDto, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskService.DeleteTaskAsync(id, userId);
            return Ok();
        }

        [HttpGet("assigned-to-me")]
        public async Task<ActionResult<List<TaskDto>>> GetTasksAssignedToMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = await _taskService.GetTasksAssignedToUserAsync(userId);
            return Ok(tasks);
        }
    }
}
