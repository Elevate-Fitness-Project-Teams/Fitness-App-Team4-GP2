using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileService.BuildingBlocks.Interfaces;
using ProfileService.Domain.Entities;
using ProfileService.Features.UpdateProfile.Dtos;

namespace ProfileService.Features.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<UpdateProfileResponse>>
    {
        private readonly IGenericRepository<UserProfile> _profileRepo;
        private readonly ICurrentUser _currentUser;
        private readonly IAuthClient _authClient;

        public UpdateProfileCommandHandler(IGenericRepository<UserProfile> profileRepo, ICurrentUser currentUser, IAuthClient authClient)
        {
            _profileRepo = profileRepo;
            _currentUser = currentUser;
            _authClient = authClient;
        }
        public async Task<Result<UpdateProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var profile = await _profileRepo.GetAllAsync(x => x.UserId == userId).FirstOrDefaultAsync();

            if (profile is null)
                return Error.NotFound("RES_NOT_FOUND.");

            if (!string.Equals(profile.Email, request.Dto.Email,
                StringComparison.OrdinalIgnoreCase))
            {
                // Change the email in AuthService first (it owns the unique constraint).
                // On failure, surface its actual error (e.g. AUTH_EMAIL_EXISTS -> 409) and stop.
                var emailResult = await _authClient.UpdateEmailAsync(request.Dto.Email, cancellationToken);

                if (!emailResult.IsSuccess)
                    return Result<UpdateProfileResponse>.Fail(emailResult.Errors);
            }

            profile.FirstName = request.Dto.FirstName;
            profile.LastName = request.Dto.LastName;
            profile.Email = request.Dto.Email;
            profile.PhoneNumber = request.Dto.PhoneNumber;

            _profileRepo.Update(profile);

            await _profileRepo.SaveChangesAsync();

            return new UpdateProfileResponse(true);
        }
    }
}
