using AuthService.Features.VerifyOtp.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.VerifyOtp
{
    public record VerifyOtpCommand(string email, string otp): IRequest<Result<VerifyOtpResponse>>;
}
