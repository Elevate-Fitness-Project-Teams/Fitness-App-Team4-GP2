using BuildingBlocks.Contracts.Progress;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService.BuildingBlock.Consumers
{
    public class SessionCompletedConsumer : IConsumer<SessionCompletedEvent>
    {
        private readonly IGenericRepository<WorkoutSession> repository;

        public SessionCompletedConsumer(IGenericRepository<WorkoutSession> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<SessionCompletedEvent> context)
        {
            var message = context.Message;

            var session = await repository
                .GetAllAsync(x => x.SessionId == message.SessionId)
                .FirstOrDefaultAsync();

            if (session is null)
                return;

            session.Status = WorkOutSessionStatus.completed;

            await repository.SaveChangesAsync();
        }
    }
}
