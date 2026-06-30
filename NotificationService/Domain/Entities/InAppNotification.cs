using System;
namespace NotificationService.Domain.Entities
{
    public class InAppNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; }
    }
}
