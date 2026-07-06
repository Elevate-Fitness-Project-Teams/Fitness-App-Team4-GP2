using BuildingBlocks.Contracts.Progress;
using BuildingBlocks.Shared.Results;
using FluentValidation;
using MassTransit;
using MassTransit.Initializers;
using MediatR;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Features.LogWeightEntry
{
    public class LogWeigEntryHandler : IRequestHandler<LogWeightEntryCommand, Result<LogWeightEntryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LogWeightEntryCommand> _validator;
        private readonly IPublishEndpoint _publishEndpoint;

        public LogWeigEntryHandler(IUnitOfWork unitOfWork , IValidator<LogWeightEntryCommand> validator ,IPublishEndpoint publishEndpoint )
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _publishEndpoint = publishEndpoint;
        }

        async Task<Result<LogWeightEntryResponse>> IRequestHandler<LogWeightEntryCommand, Result<LogWeightEntryResponse>>.Handle(LogWeightEntryCommand request, CancellationToken cancellationToken)
        {

             
            var validationResult = await _validator.ValidateAsync(request , cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.Errors.Select(e => Error.Validation("One or more validation errors occurred", e.ErrorMessage)).ToList();
             
            var userId = request.UserId;

            var WeightHistoryRepository = _unitOfWork.GetRepository<WeightHistory, int>();
            var previousWeight =  WeightHistoryRepository.GetAll(x=> x.UserId == userId)
                .OrderByDescending(x => x.Date).Select(x=>x.Weight).FirstOrDefault();

            var weight = new WeightHistory
            {
                UserId = request.UserId,
                Weight = request.Weight,
                Date = request.Date,
                Notes = request.Notes
            };
            _unitOfWork.GetRepository<WeightHistory, int>().Add(weight);

            var differenceFromPrevious = previousWeight == 0 ? 0: previousWeight - request.Weight;

            var userStatistic = await _unitOfWork.GetRepository<UserStatistic, int>().GetOneAsync(x => x.UserId == userId);
            if (userStatistic is null)
                return Error.NotFound("NotFound" , "UserStatistics is not found");
            
            userStatistic.TotalWeightLost += differenceFromPrevious;
           
            await _unitOfWork.SaveChangesAsync();

            await _publishEndpoint.Publish(
                   new WeightUpdatedEvent
                   {
                       UserId = request.UserId,
                       WeightKg = request.Weight,
                       RecordedAt = request.Date,
                   });

            // Publish the full current stats so ProfileService's cached snapshot stays correct
            // (the event carries complete state, not a delta).
            var streak = await _unitOfWork.GetRepository<Streak, int>().GetOneAsync(x => x.UserId == userId);

            await _publishEndpoint.Publish(new UserStatisticsUpdatedEvent
            {
                UserId = userId,
                TotalWorkouts = userStatistic.TotalWorkouts,
                CurrentStreak = streak?.CurrentStreak ?? 0,
                LongestStreak = streak?.LongestStreak ?? 0,
                TotalCaloriesBurned = userStatistic.TotalCaloriesBurned,
                TotalWeightLost = userStatistic.TotalWeightLost,
                UpdatedAt = DateTime.UtcNow
            });

            return new LogWeightEntryResponse
            {
                DifferenceFromPrevious = differenceFromPrevious,
                TotalWeightLost = userStatistic.TotalWeightLost
            };
        }
    }
}
