using BuildingBlocks.Contracts.Profile;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ProfileService.Infrastructure.Persistence.Consumers
{
    /// <summary>
    /// Keeps the cached email in sync when AuthService changes it. Acts as a self-healing
    /// backstop: even if a synchronous profile update failed to persist locally after the
    /// AuthService email change succeeded, this event brings the cached copy back in line.
    /// </summary>
    public class UserEmailChangedConsumer : IConsumer<UserEmailChangedEvent>
    {
        private readonly ProfileDbContext _db;

        public UserEmailChangedConsumer(ProfileDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<UserEmailChangedEvent> context)
        {
            var message = context.Message;

            var profile = await _db.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == message.UserId);

            if (profile is null || profile.Email == message.NewEmail)
                return;

            profile.Email = message.NewEmail;
            await _db.SaveChangesAsync();
        }
    }
}
