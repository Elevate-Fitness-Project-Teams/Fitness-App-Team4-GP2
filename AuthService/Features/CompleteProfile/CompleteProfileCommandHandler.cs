using AuthService.BuildingBlocks.Exceptions;
using BuildingBlocks.Contracts.Auth;
using AuthService.BuildingBlocks.Interfaces;
using AuthService.BuildingBlocks.Interfaces.Events;
using AuthService.Features.CompleteProfile;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class CompleteProfileCommandHandler
    : IRequestHandler<CompleteProfileCommand, bool>
{
    private readonly ICurrentUser _currentUser;
    private readonly IApplicationDbContext _context;
    private readonly IEventPublisher _publisher;

    public CompleteProfileCommandHandler(ICurrentUser currentUser, IApplicationDbContext context,
        IEventPublisher publisher)
    {
        _currentUser = currentUser;
        _context = context;
        _publisher = publisher;
    }

    public async Task<bool> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUser.UserId, out var userId))
            throw new UnauthorizedException("AUTH_TOKEN_INVALID.");

        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            throw new UnauthorizedException("AUTH_TOKEN_INVALID.");

        if (!user.RequiresProfileCompletion)
            return false;

        user.RequiresProfileCompletion = false;

        await _context.SaveChangesAsync(cancellationToken);

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