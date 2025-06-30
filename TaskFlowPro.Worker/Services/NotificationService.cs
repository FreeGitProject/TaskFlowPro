using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlowPro.Api.Data;
using TaskFlowPro.Api.Models;
using TaskStatus= TaskFlowPro.Api.Models.TaskStatus;
namespace TaskFlowPro.Worker.Services
{
    public interface INotificationService
    {
        System.Threading.Tasks.Task CheckOverdueTasksAsync();
        System.Threading.Tasks.Task SendDueDateRemindersAsync();
    }

    public class NotificationService : INotificationService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IDbContextFactory<AppDbContext> dbContextFactory,
            ILogger<NotificationService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task CheckOverdueTasksAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();

            var overdueTasks = await context.Tasks
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value < DateTime.UtcNow &&
                           t.Status != TaskStatus.Completed &&
                           t.Status != TaskStatus.Cancelled)
                .Include(t => t.AssignedTo)
                .ToListAsync();

            foreach (var task in overdueTasks)
            {
                var notification = new Notification
                {
                    UserId = task.AssignedToId,
                    Message = $"Task '{task.Title}' is overdue!",
                    RelatedEntityType = "Task",
                    RelatedEntityId = task.Id
                };

                context.Notifications.Add(notification);
                _logger.LogInformation($"Created overdue notification for task {task.Id}");
            }

            await context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task SendDueDateRemindersAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();

            var dueSoonTasks = await context.Tasks
                .Where(t => t.DueDate.HasValue &&
                          t.DueDate.Value > DateTime.UtcNow &&
                          t.DueDate.Value <= DateTime.UtcNow.AddHours(24) &&
                          t.Status != TaskStatus.Completed &&
                          t.Status != TaskStatus.Cancelled)
                .Include(t => t.AssignedTo)
                .ToListAsync();

            foreach (var task in dueSoonTasks)
            {
                var notification = new Notification
                {
                    UserId = task.AssignedToId,
                    Message = $"Task '{task.Title}' is due soon ({task.DueDate?.ToString("g")})",
                    RelatedEntityType = "Task",
                    RelatedEntityId = task.Id
                };

                context.Notifications.Add(notification);
                _logger.LogInformation($"Created due soon notification for task {task.Id}");
            }

            await context.SaveChangesAsync();
        }
    }
}
