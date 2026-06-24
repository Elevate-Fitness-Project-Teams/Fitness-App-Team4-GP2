using AuthService.Domain.Entities;
using AuthService.Features.Auth.Register;
using AuthService.Features.Register.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new ConflictException("AUTH_EMAIL_EXISTS");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            RequiresProfileCompletion = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        return new RegisterResponse(user.Id, true);
    }
}