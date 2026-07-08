namespace ProfileService.Features.ChangePassword.Dtos
{
    public record ChangePasswordRequest(string CurrentPassword,
    string NewPassword,
    string ConfirmPassword);
}
