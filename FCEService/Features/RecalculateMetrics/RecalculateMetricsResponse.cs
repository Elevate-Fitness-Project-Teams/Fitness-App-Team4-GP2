namespace FCEService.Features.RecalculateMetrics
{
    
        public sealed record MetricsSnapshot(double Bmr, double Tdee, double CalorieTarget, string Status);

        public sealed record RecalculateMetricsResponse(
            Guid UserId, MetricsSnapshot PreviousMetrics, MetricsSnapshot NewMetrics, bool PlanReassignment);
}
