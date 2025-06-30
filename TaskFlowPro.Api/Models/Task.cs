using System.ComponentModel.DataAnnotations;

namespace TaskFlowPro.Api.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.New;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [Required]
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public string AssignedToId { get; set; }
        public User AssignedTo { get; set; }
        public DateTime UpdatedAt { get; internal set; }
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
