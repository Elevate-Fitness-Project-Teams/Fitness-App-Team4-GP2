using BuildingBlocks.Shared.Results;
using MediatR;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Features.ViewProgressStats
{
    public class ViewProgressStatsHandler : IRequestHandler<ViewProgressStatsQuery, Result<ViewProgressStatsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewProgressStatsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ViewProgressStatsResponse>> Handle(ViewProgressStatsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var user = await _unitOfWork.GetRepository<UserReadModel, int>().GetOneAsync(x => x.UserId == userId);
            if (user is null)
                return Error.NotFound("NotFound", "RES_USER_NOT_FOUND.");

            var statistics = await _unitOfWork.GetRepository<UserStatistic, int>().GetOneAsync(x => x.UserId == userId);
            if (statistics is null)
                return Error.NotFound("NotFound", "RES_USER_STATISTICS_NOT_FOUND.");

            var streaks = await _unitOfWork.GetRepository<Streak, int>().GetOneAsync(x => x.UserId == userId);
            if (streaks is null)
                return Error.NotFound("NotFound", "RES_USER_STREAKS_NOT_FOUND.");

            var response = new ViewProgressStatsResponse
            {
                TotalWorkouts = statistics.TotalWorkouts,
                TotalCaloriesBurned = statistics.TotalCaloriesBurned,
                TotalWeightLost = statistics.TotalWeightLost,
                CurrentStreak = streaks.CurrentStreak,
                LongestStreak = streaks.LongestStreak
            };

            return response;
        }
        }
}
