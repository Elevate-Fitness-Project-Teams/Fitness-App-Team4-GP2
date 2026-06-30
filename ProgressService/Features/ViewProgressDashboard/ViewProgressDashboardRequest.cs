namespace ProgressService.Features.ViewProgressDashboard
{
    public record ViewProgressDashboardRequest (Period? Period, DateTime? StartDate, DateTime? EndDate);


    public enum Period
    {
        Weekly,
        Monthly,
        Yearly,
        All
        
    }
}
