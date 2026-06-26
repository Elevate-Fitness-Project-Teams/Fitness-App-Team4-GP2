namespace AuthService.Features.Login.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool ProfileCompleted { get; set; }
        public bool IsPremium { get; set; }
    }
}
