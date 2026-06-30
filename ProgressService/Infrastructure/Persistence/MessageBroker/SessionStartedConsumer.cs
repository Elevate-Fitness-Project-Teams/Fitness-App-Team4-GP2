using BuildingBlocks.Contracts.Progress;
using MassTransit;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Infrastructure.Persistence.MessageBroker
{
    public class SessionStartedConsumer : IConsumer<SessionStartedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionStartedConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<SessionStartedEvent> context)
        {
            var session = new SessionReadModel
            {
                SessionId = context.Message.SessionId,
                UserId = context.Message.UserId,
                IsActive = true
            };
             _unitOfWork.GetRepository<SessionReadModel,string>().Add(session);
             await _unitOfWork.SaveChangesAsync();
        }
    }
}
