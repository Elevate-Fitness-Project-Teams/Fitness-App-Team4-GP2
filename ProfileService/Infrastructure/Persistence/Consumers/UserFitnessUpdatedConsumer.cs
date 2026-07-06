using BuildingBlocks.Contracts.Fce;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;

namespace ProfileService.Infrastructure.Persistence.Consumers
{
    /// <summary>
    /// Refreshes the fitness portion (height/weight) of the cached snapshot from FCEService events.
    /// </summary>
    public class UserFitnessUpdatedConsumer : IConsumer<UserFitnessUpdatedEvent>
    {
        private readonly ProfileDbContext _db;

        public UserFitnessUpdatedConsumer(ProfileDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<UserFitnessUpdatedEvent> context)
        {
            var message = context.Message;

            var snapshot = await _db.UserStatisticsSnapshots
                .FirstOrDefaultAsync(s => s.UserId == message.UserId);

            if (snapshot is null)
            {
                snapshot = new UserStatisticsSnapshot { UserId = message.UserId };
                await _db.UserStatisticsSnapshots.AddAsync(snapshot);
            }
            else if (snapshot.FitnessUpdatedAt is { } last && last >= message.UpdatedAt)
            {
                // Ignore stale / out-of-order deliveries.
                return;
            }

            snapshot.Weight = message.Weight;
            snapshot.Height = message.Height;
            snapshot.FitnessUpdatedAt = message.UpdatedAt;

            await _db.SaveChangesAsync();
        }
    }
}
