using BuildingBlocks.Contracts.Progress;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;

namespace ProfileService.Infrastructure.Persistence.Consumers
{
    /// <summary>
    /// Refreshes the progress-stats portion of the cached snapshot from ProgressService events.
    /// </summary>
    public class UserStatisticsUpdatedConsumer : IConsumer<UserStatisticsUpdatedEvent>
    {
        private readonly ProfileDbContext _db;

        public UserStatisticsUpdatedConsumer(ProfileDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<UserStatisticsUpdatedEvent> context)
        {
            var message = context.Message;

            var snapshot = await _db.UserStatisticsSnapshots
                .FirstOrDefaultAsync(s => s.UserId == message.UserId);

            if (snapshot is null)
            {
                snapshot = new UserStatisticsSnapshot { UserId = message.UserId };
                await _db.UserStatisticsSnapshots.AddAsync(snapshot);
            }
            else if (snapshot.StatsUpdatedAt is { } last && last >= message.UpdatedAt)
            {
                // Ignore stale / out-of-order deliveries.
                return;
            }

            snapshot.TotalWorkouts = message.TotalWorkouts;
            snapshot.CurrentStreak = message.CurrentStreak;
            snapshot.LongestStreak = message.LongestStreak;
            snapshot.TotalCaloriesBurned = message.TotalCaloriesBurned;
            snapshot.TotalWeightLost = message.TotalWeightLost;
            snapshot.StatsUpdatedAt = message.UpdatedAt;

            await _db.SaveChangesAsync();
        }
    }
}
