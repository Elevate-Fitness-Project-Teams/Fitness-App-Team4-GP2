using BuildingBlocks.Contracts.Subscription;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProfileService.Domain.Entities;

namespace ProfileService.Infrastructure.Persistence.Consumers
{
    /// <summary>
    /// Consumes SubscriptionUpgradedEvent from SubscriptionService and updates the user's cached premium status.
    /// </summary>
    public class SubscriptionUpgradedConsumer : IConsumer<SubscriptionUpgradedEvent>
    {
        private readonly ProfileDbContext _db;
        private readonly ILogger<SubscriptionUpgradedConsumer> _logger;

        public SubscriptionUpgradedConsumer(ProfileDbContext db, ILogger<SubscriptionUpgradedConsumer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubscriptionUpgradedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Processing SubscriptionUpgradedEvent for UserId: {UserId}, Plan: {Plan}, IsPremium: {IsPremium}",
                message.UserId, message.Plan, message.IsPremium);

            var profile = await _db.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == message.UserId);

            if (profile is null)
            {
                _logger.LogWarning("UserProfile not found for UserId: {UserId}. Cannot update premium status cache.", message.UserId);
                return;
            }

            profile.IsPremiumCached = message.IsPremium;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Successfully updated IsPremiumCached to {IsPremium} for UserId: {UserId}",
                message.IsPremium, message.UserId);
        }
    }
}

