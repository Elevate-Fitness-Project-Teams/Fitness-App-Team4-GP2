namespace ProfileService.Features.ViewSettings.Dtos
{
    public class NotificationSettingsDto
    {
        public bool WorkoutReminders { get; set; }
        public bool MealReminders { get; set; } 
        public bool AchievementAlerts { get; set; }
        public bool WeeklyReports { get; set; } 
        public bool EmailNotifications { get; set; } 
        public bool PushNotifications { get; set; } 
    }
}
