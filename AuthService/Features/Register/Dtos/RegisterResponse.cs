namespace AuthService.Features.Register.Dtos
{
    public record RegisterResponse(string UserId, bool RequiresProfileCompletion);
}
