using BuildingBlocks.Contracts.Auth;
using AuthService.BuildingBlocks.Interfaces;
using AuthService.BuildingBlocks.Interfaces.Events;
using AuthService.Features.CompleteProfile;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;

public sealed class CompleteProfileCommandHandler
    : IRequestHandler<CompleteProfileCommand, Result<bool>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IGenericRepository<ApplicationUser> _userRepository;
    private readonly IEventPublisher _publisher;

    public CompleteProfileCommandHandler(ICurrentUser currentUser, IGenericRepository<ApplicationUser> userRepository,
        IEventPublisher publisher)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _publisher = publisher;
    }

    public async Task<Result<bool>> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUser.UserId, out var userId))
            return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
            return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

        if (!user.RequiresProfileCompletion)
            return false;

        user.RequiresProfileCompletion = false;

        await _userRepository.SaveChangesAsync();

        await _publisher.PublishAsync(
            new UserProfileCompletedEvent
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                IsPremium = user.IsPremium,
                CompletedAt = DateTime.UtcNow
            });

        return true;
    }
}