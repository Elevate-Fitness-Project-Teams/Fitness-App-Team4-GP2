using BuildingBlocks.Contracts.Progress;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Consumers
{
    /// <summary>
    /// Consumes AchievementEarnedEvent from ProgressService and creates an in-app notification.
    /// </summary>
    public class AchievementEarnedConsumer : IConsumer<AchievementEarnedEvent>
    {
        private readonly NotificationDbContext _db;
        private readonly ILogger<AchievementEarnedConsumer> _logger;

        public AchievementEarnedConsumer(NotificationDbContext db, ILogger<AchievementEarnedConsumer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AchievementEarnedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Received AchievementEarnedEvent: UserId={UserId}, Achievement={AchievementName}",
                message.UserId, message.AchievementName);

            if (!Guid.TryParse(message.UserId, out var userId))
            {
                _logger.LogWarning("Invalid UserId format in AchievementEarnedEvent: {UserId}", message.UserId);
                return;
            }

            var notificationMessage = $"Congratulations! You've unlocked {message.AchievementName}.";

            // Prevent duplicate notifications if already delivered
            var exists = await _db.InAppNotifications
                .AnyAsync(n => n.UserId == userId && n.Message == notificationMessage);

            if (exists)
            {
                _logger.LogInformation("Notification already exists for UserId={UserId}, Achievement={AchievementName}. Skipping.",
                    userId, message.AchievementName);
                return;
            }

            var notification = new InAppNotification
            {
                UserId = userId,
                Title = "Achievement Unlocked!",
                Message = notificationMessage,
                Type = NotificationType.AchievementAlert,
                IsRead = false,
                SentAt = message.EarnedAt != default ? message.EarnedAt : DateTime.UtcNow
            };

            await _db.InAppNotifications.AddAsync(notification);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Created achievement notification {NotificationId} for UserId={UserId}",
                notification.Id, userId);
        }
    }
}

