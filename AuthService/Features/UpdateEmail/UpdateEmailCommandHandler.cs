using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using BuildingBlocks.Shared.Results;
using BuildingBlocks.Contracts.Profile;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Features.UpdateEmail
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPublishEndpoint _publish;
        private readonly ICurrentUser _currentUser;

        public UpdateEmailCommandHandler(UserManager<ApplicationUser> userManager, IPublishEndpoint publish, ICurrentUser currentUser)
        {
            _userManager = userManager;
            _publish = publish;
            _currentUser = currentUser;
        }
        public async Task<Result<bool>> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            // Identity comes from the authenticated token, never a caller-supplied id.
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Error.Unauthorized("AUTH_USER_NOT_FOUND");

            var exists = await _userManager.FindByEmailAsync(request.NewEmail);

            if (exists != null && exists.Id != user.Id)
                return Error.Conflict("AUTH_EMAIL_EXISTS");

            var oldEmail = user.Email;

            user.Email = request.NewEmail;
            user.UserName = request.NewEmail;

            user.NormalizedEmail = _userManager.NormalizeEmail(request.NewEmail);
            user.NormalizedUserName = _userManager.NormalizeName(request.NewEmail);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Error.Validation("AUTH_EMAIL_UPDATE_FAILED");

            await _publish.Publish(new UserEmailChangedEvent
            {
                UserId = user.Id,
                OldEmail = oldEmail!,
                NewEmail = request.NewEmail
            });

            return true;
        }
    }
}
