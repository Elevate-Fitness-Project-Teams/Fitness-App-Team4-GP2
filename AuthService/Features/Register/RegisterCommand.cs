using AuthService.Features.Register.Dtos;
using BuildingBlocks.Shared.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.Auth.Register;

// Validation is performed manually in RegisterCommandHandler (no DataAnnotations pipeline).
public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    [RegularExpression(
        @"^(\+20|0)?1[0125]\d{8}$",
        ErrorMessage = "Invalid Egyptian phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;
}