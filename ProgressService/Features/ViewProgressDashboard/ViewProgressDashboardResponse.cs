using ProgressService.Features.ViewProgressDashboard.DTOs;

namespace ProgressService.Features.ViewProgressDashboard
{
    public class ViewProgressDashboardResponse
    {
        public StatisticsSummaryDto StatisticsSummary { get; set; } = default!;
        public List<WeightHistoryDto> WeightHistory { get; set; } = [];
        public List<WorkoutHistoryDto> WorkoutHistory { get; set; } = [];
        public WeekComparisonsDto WeekComparisons { get; set; } = default!;
        public List<AchievementsDto> Achievements { get; set; } = [];
    }
}