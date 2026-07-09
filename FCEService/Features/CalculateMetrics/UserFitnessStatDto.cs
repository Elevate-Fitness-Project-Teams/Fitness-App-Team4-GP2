using FCEService.Domain.Enums;

namespace FCEService.Features.CalculateMetrics
{
    public sealed record UserFitnessStatDto(
        Guid UserId, 
        double Weight,
        double Height,
        int Age,
        Gender Gender, 
        ActivityLevel ActivityLevel,
        FitnessGoal Goal, 
        DateTime RecordedAt);

}
