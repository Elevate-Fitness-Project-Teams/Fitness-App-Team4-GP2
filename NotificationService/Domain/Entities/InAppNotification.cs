using System;
using System.Text.Json.Serialization;
namespace NotificationService.Domain.Entities
{
    public class InAppNotification
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationType
    {
        AchievementAlert,
        StreakWarning,
        SystemUpdate
    }
}
