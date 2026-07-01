namespace FCEService.Features.Shared
{
    // Shared Request DTO for endpoints that only need a UserId in the body
    public sealed record UserIdRequest(Guid UserId);
}
