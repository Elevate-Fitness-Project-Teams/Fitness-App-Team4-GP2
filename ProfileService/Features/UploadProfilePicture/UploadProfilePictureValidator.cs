using FluentValidation;

namespace ProfileService.Features.UploadProfilePicture
{
    public sealed class UploadProfilePictureValidator : AbstractValidator<UploadProfilePictureCommand>
    {
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public UploadProfilePictureValidator()
        {
            RuleFor(x => x.ProfilePicture)
                .Must(f => f is not null && f.Length > 0)
                .WithErrorCode("VAL_INVALID_FILE_TYPE")
                .WithMessage("No file was uploaded.");

            When(x => x.ProfilePicture is not null && x.ProfilePicture.Length > 0, () =>
            {
                RuleFor(x => x.ProfilePicture)
                    .Must(f => AllowedExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
                    .WithErrorCode("VAL_INVALID_FILE_TYPE")
                    .WithMessage("Only JPG and PNG files are allowed.");

                RuleFor(x => x.ProfilePicture)
                    .Must(f => f.Length <= MaxFileSizeBytes)
                    .WithErrorCode("VAL_FILE_TOO_LARGE")
                    .WithMessage("The file must not exceed 5 MB.");
            });
        }
    }
}
