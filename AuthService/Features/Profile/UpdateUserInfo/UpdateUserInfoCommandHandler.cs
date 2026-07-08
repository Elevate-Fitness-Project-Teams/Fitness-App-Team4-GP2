using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using BuildingBlocks.Contracts.Profile;
using BuildingBlocks.Shared.Results;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Features.Profile.UpdateUserInfo
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser _currentUser;
        private readonly IPublishEndpoint _publish;

        public UpdateUserInfoCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUser currentUser,
            IPublishEndpoint publish)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _publish = publish;
        }

        public async Task<Result<bool>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            // Identity comes from the authenticated token, never a caller-supplied id.
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Error.Unauthorized("AUTH_USER_NOT_FOUND");

            var emailChanged = !string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase);
            var oldEmail = user.Email;

            // Email is the unique identity field — enforce uniqueness before changing it.
            if (emailChanged)
            {
                var existing = await _userManager.FindByEmailAsync(request.Email);
                if (existing is not null && existing.Id != user.Id)
                    return Error.Conflict("AUTH_EMAIL_EXISTS", "This email is already in use.");

                user.Email = request.Email;
                user.NormalizedEmail = _userManager.NormalizeEmail(request.Email);
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            // UserName is ignored in the DB model, so it loads back null and would fail Identity's
            // user validator on update. Keep it aligned with the (possibly new) email.
            user.UserName = user.Email;
            user.NormalizedUserName = _userManager.NormalizeName(user.Email);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Error.Failure("AUTH_USER_UPDATE_FAILED", "Could not update user information.");

            // Let ProfileService's cached copy self-heal if the email changed.
            if (emailChanged)
                await _publish.Publish(new UserEmailChangedEvent
                {
                    UserId = user.Id,
                    OldEmail = oldEmail!,
                    NewEmail = request.Email
                });

            return true;
        }
    }
}
