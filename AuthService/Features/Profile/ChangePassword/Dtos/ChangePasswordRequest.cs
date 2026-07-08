namespace AuthService.Features.Profile.ChangePassword.Dtos
{
    public record ChangePasswordRequest(string CurrentPassword,
    string NewPassword,
    string ConfirmPassword);
}
