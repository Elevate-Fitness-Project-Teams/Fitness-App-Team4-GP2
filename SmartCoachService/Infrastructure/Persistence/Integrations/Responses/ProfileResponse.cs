namespace SmartCoachService.Infrastructure.Persistence.Integrations.Responses
{
    // Mirrors ProfileService.ViewProfileResponse returned by GET /api/v1/profile.
    // Extra upstream fields (e.g. ProgressStatistics) are ignored during deserialization.
    public record ProfileResponse(
        Guid UserId,
        string FirstName,
        string LastName,
        ProfileFceDto? FceProfile,
        string? ProfilePictureUrl);

    public record ProfileFceDto(double? Height, double? Weight);
}
