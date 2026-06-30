using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.ForgotPassword.Dtos
{
    public record ForgotPasswordResponse(
    [EmailAddress] string Email,
    int OtpExpiresIn,
    int CanResendIn);
}
