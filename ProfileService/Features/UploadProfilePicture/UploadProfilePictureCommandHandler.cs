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
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

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

            var validation = ValidatePicture(request.ProfilePicture);
            if (validation is not null)
                return validation;

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


        private static Error? ValidatePicture(IFormFile? file)
        {
            if (file is null || file.Length == 0)
                return Error.Validation("VAL_INVALID_FILE_TYPE", "No file was uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return Error.Validation("VAL_INVALID_FILE_TYPE", "Only JPG and PNG files are allowed.");

            if (file.Length > MaxFileSizeBytes)
                return Error.Validation("VAL_FILE_TOO_LARGE", "The file must not exceed 5 MB.");

            return null;
        }
    }
}
