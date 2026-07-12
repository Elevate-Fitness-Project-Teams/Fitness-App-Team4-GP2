using AuthService.Features.Register.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Auth.Register;

// Validation is performed manually in RegisterCommandHandler (no DataAnnotations pipeline).
public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}