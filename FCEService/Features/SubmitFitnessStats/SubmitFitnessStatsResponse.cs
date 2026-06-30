namespace FCEService.Features.SaveFitnessStats
{
    public sealed record SubmitFitnessStatsResponse(
        int Id,
        Guid UserId,
        double Weight,
        double Height,
        int Age,
        string Gender,
        string Goal,
        string ActivityLevel,
        DateTime RecordedAt
    );
}
