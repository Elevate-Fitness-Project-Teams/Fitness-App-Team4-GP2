using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileService.BuildingBlocks.Interfaces;
using ProfileService.Domain.Entities;
using ProfileService.Features.ViewProfile.Dtos;

namespace ProfileService.Features.ViewProfile
{
    public class ViewProfileQueryHandler : IRequestHandler<ViewProfileQuery, Result<ViewProfileResponse>>
    {
        private readonly IGenericRepository<UserProfile> _profiles;
        private readonly IGenericRepository<UserStatisticsSnapshot> _snapshots;
        private readonly ICurrentUser _currentUser;

        public ViewProfileQueryHandler(
            IGenericRepository<UserProfile> profiles,
            IGenericRepository<UserStatisticsSnapshot> snapshots,
            ICurrentUser currentUser)
        {
            _profiles = profiles;
            _snapshots = snapshots;
            _currentUser = currentUser;
        }

        public async Task<Result<ViewProfileResponse>> Handle(ViewProfileQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var profile = await _profiles.GetAllAsync(x => x.UserId == userId).FirstOrDefaultAsync();

            if (profile is null)
                return Error.NotFound("RES_NOT_FOUND");

            // Cached snapshot fed asynchronously by ProgressService + FCEService events.
            var stats = await _snapshots.GetAllAsync(x => x.UserId == userId).FirstOrDefaultAsync();

            return new ViewProfileResponse
            {
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfilePictureUrl = profile.ProfilePictureUrl,

                FceProfile = new FceProfileDto
                {
                    Height = stats?.Height,
                    Weight = stats?.Weight
                },

                ProgressStatistics = new ProgressStatisticsDto
                {
                    TotalWorkouts = stats?.TotalWorkouts ?? 0,
                    CurrentStreak = stats?.CurrentStreak ?? 0,
                    LongestStreak = stats?.LongestStreak ?? 0,
                    TotalCaloriesBurned = stats?.TotalCaloriesBurned ?? 0,
                    TotalWeightLost = stats?.TotalWeightLost ?? 0
                }
            };
        }
    }
}
