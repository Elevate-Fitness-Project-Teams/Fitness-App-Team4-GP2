namespace AuthService.Features.RefreshToken.Dtos
{
    public record RefreshTokenResponse(string accessToken, string refreshToken);
}
