using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;
using ProgressService.Features.ViewProgressDashboard.DTOs;

namespace ProgressService.Features.ViewProgressDashboard
{
    public class ViewProgressDashboardHandler : IRequestHandler<ViewProgressDashboardQuery, Result<ViewProgressDashboardResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewProgressDashboardHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ViewProgressDashboardResponse>> Handle(ViewProgressDashboardQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate > request.EndDate)
                return Error.Validation("InvalidDateRange", "VAL_INVALID_DATE");

            var userId = request.UserId;

            var endDate = request.EndDate ?? DateTime.UtcNow;
            var startDate = request.StartDate;
            if(startDate is null)
            {
                startDate = request.Period switch
                {
                    Period.Weekly => endDate.AddDays(-7),
                    Period.Monthly => endDate.AddMonths(-1),
                    Period.Yearly => endDate.AddYears(-1),
                    _ => DateTime.MinValue
                };

            }

            var Userstatistics= await _unitOfWork.GetRepository<UserStatistic, int>().GetOneAsync(x => userId == x.UserId);


            var WeightHistory =await _unitOfWork.GetRepository<WeightHistory, int>().GetAll(x => userId == x.UserId && x.Date >= startDate && x.Date <= endDate)
                .Select(x=> new WeightHistoryDto(x.Weight, x.Date)).ToListAsync();

            var WorkoutHistory = await _unitOfWork.GetRepository<WorkoutLog, int>().GetAll(x => userId == x.UserId && x.CompletedAt >= startDate && x.CompletedAt <= endDate)
                .Select(x => new WorkoutHistoryDto(x.Id, x.DurationInMinutes, x.CaloriesBurned, x.CompletedAt)).ToListAsync();
            var Achievements = await _unitOfWork.GetRepository<UserAchievement, int>()
                .GetAll(x => userId == x.UserId && x.EarnedAt >= startDate && x.EarnedAt <= endDate).Select(x => new AchievementsDto
                (
                    x.AchievementId,
                    x.Achievement.Name,
                    x.Achievement.IconUrl,
                    x.Achievement.CriteriaType


                )).ToListAsync();
            var currentWeek =  endDate.AddDays(-7);
            var previousWeek = currentWeek.AddDays(-7);

            var currentWeekWorkouts = WorkoutHistory.Count(x=>x.CompletedAt >= currentWeek);
            var previousWeekWorkouts = WorkoutHistory.Count(x=>x.CompletedAt >= previousWeek && x.CompletedAt < currentWeek);
            var PercentageChange = previousWeekWorkouts == 0 ? 100 : ((double)(currentWeekWorkouts - previousWeekWorkouts) / previousWeekWorkouts) * 100;

            var Comparison = new WeekComparisonsDto(currentWeekWorkouts, previousWeekWorkouts, PercentageChange);

            return new ViewProgressDashboardResponse
            {
                StatisticsSummary = new StatisticsSummaryDto
                (
                  Userstatistics?.TotalWorkouts ?? 0,
                  Userstatistics?.TotalCaloriesBurned ?? 0,
                  Userstatistics?.TotalWeightLost ?? 0,
                  Userstatistics?.UpdatedAt ?? DateTime.UtcNow
                ),
                WeightHistory = WeightHistory,
                WorkoutHistory = WorkoutHistory,
                Achievements = Achievements,
                WeekComparisons = Comparison

            };





        }
    }
}
