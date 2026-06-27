using AuthService.Features.Register.Dtos;
using AuthService.Shared.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Features.Auth.Register;

public class RegisterCommand : IRequest<RegisterResponse>
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d).{6,}$",
        ErrorMessage = "Password must be at least 6 characters and contain one uppercase letter and one number.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^(\+20|0)?1[0125]\d{8}$",
        ErrorMessage = "Invalid Egyptian phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;
}