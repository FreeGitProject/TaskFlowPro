namespace TaskFlowPro.Api.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
    }
}
