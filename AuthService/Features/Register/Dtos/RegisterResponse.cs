namespace AuthService.Features.Register.Dtos
{
    public record RegisterResponse(Guid UserId, bool RequiresProfileCompletion);
}
