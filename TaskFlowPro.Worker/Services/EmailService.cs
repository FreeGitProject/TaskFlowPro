using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlowPro.Worker.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // In a real application, implement actual email sending logic
            // This is just a mock implementation for demonstration
            _logger.LogInformation($"Sending email to {email} with subject '{subject}': {message}");
            return Task.CompletedTask;
        }
    }
}
