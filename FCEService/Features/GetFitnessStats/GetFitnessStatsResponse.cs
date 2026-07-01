namespace FCEService.Features.GetFitnessStats
{
    public sealed record GetFitnessStatsResponse(
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
