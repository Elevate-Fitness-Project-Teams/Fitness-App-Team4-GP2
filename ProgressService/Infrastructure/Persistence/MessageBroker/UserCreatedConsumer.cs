using BuildingBlocks.Contracts.Auth;
using MassTransit;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Infrastructure.Persistence.MessageBroker
{
    // Must implement IConsumer<T>: Program.cs registers consumers with
    // AddConsumers(assembly), which discovers by interface. Without it this class was
    // never wired to the bus, so UserReadModel stayed empty and every progress lookup
    // returned RES_USER_NOT_FOUND.
    public class UserCreatedConsumer : IConsumer<UserProfileCompletedEvent>
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
