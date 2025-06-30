using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlowPro.Worker.Services;

namespace TaskFlowPro.Worker.Workers
{
    public class TaskProcessingWorker : BackgroundService
    {
        private readonly ILogger<TaskProcessingWorker> _logger;
        private readonly IServiceProvider _services;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public TaskProcessingWorker(
            ILogger<TaskProcessingWorker> logger,
            IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Processing Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    await notificationService.CheckOverdueTasksAsync();
                    await notificationService.SendDueDateRemindersAsync();

                    _logger.LogInformation("Completed task processing cycle");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during task processing");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Task Processing Worker stopped");
        }
    }
}
