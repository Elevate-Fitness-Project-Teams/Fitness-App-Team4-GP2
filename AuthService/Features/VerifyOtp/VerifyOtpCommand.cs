using AuthService.Features.VerifyOtp.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.VerifyOtp
{
    public record VerifyOtpCommand([EmailAddress] string email, string otp): IRequest<Result<VerifyOtpResponse>>;
}
