using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileService.BuildingBlocks.Interfaces;
using ProfileService.Domain.Entities;
using ProfileService.Features.UploadProfilePicture.Dto;

namespace ProfileService.Features.UploadProfilePicture
{
    public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, Result<UploadProfilePictureResponse>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IGenericRepository<UserProfile> _profileRepo;
        private readonly IFileStorageService _storageService;

        public UploadProfilePictureCommandHandler(ICurrentUser currentUser, IGenericRepository<UserProfile> profileRepo, IFileStorageService storageService)
        {
            _currentUser = currentUser;
            _profileRepo = profileRepo;
            _storageService = storageService;
        }
        public async Task<Result<UploadProfilePictureResponse>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return Error.Unauthorized("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var profile = await _profileRepo
            .GetAllAsync(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken);

            if (profile == null)
                return Error.NotFound("RES_NOT_FOUND");

            try
            {
                var url = await _storageService.SaveProfilePictureAsync(request.ProfilePicture);

                profile.ProfilePictureUrl = url;

                _profileRepo.Update(profile);

                await _profileRepo.SaveChangesAsync();

                return new UploadProfilePictureResponse(url);
            }
            catch
            {
                return Error.Failure("SRV_FILE_UPLOAD_FAILED", "Could not upload file.");
            }
        }
    }
}
