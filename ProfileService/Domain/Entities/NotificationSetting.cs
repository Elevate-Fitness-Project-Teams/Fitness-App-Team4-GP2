namespace ProfileService.Domain.Entities
{
    public class NotificationSetting
    {
        public Guid UserId { get; set; }
        public bool WorkoutReminders { get; set; } = true;
        public bool MealReminders { get; set; } = true;
        public bool AchievementAlerts { get; set; } = true;
        public bool WeeklyReports { get; set; } = true;
        public bool EmailNotifications { get; set; } = true;
        public bool PushNotifications { get; set; } = true;
        // Navigation property to UserProfile
        public UserProfile UserProfile { get; set; } = null!;
    }
}
