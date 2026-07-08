using AuthService.Features.UpdateEmail.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.UpdateEmail
{
    public record UpdateEmailCommand(string NewEmail) : IRequest<Result<bool>>;
}
