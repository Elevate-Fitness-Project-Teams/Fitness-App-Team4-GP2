using BuildingBlocks.Contracts.Auth;
using MassTransit;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Infrastructure.Persistence.MessageBroker
{
    public class UserCreatedConsumer
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCreatedConsumer(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserProfileCompletedEvent> context)
        {
            var User = await _unitOfWork.GetRepository<UserReadModel,int>().GetOneAsync(x => x.UserId == context.Message.UserId);

            if (User is not null)
                return;
            
            var newUser = new UserReadModel
            {
                UserId = context.Message.UserId,
                IsPremium = context.Message.IsPremium
            };
            _unitOfWork.GetRepository<UserReadModel,int>().Add(newUser);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
