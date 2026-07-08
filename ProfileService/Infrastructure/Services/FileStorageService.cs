using ProfileService.BuildingBlocks.Interfaces;

namespace ProfileService.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _root;

        public FileStorageService(IConfiguration configuration)
        {
            _root = GetUploadsRoot(configuration);
        }

        /// <summary>
        /// Uploads are stored OUTSIDE the project/content root on purpose: writing files into
        /// wwwroot while debugging triggers Visual Studio / dotnet-watch file watchers, which
        /// restart the app mid-request. Configurable via "Storage:ProfilePicturesRoot".
        /// </summary>
        public static string GetUploadsRoot(IConfiguration configuration)
        {
            var configured = configuration["Storage:ProfilePicturesRoot"];

            return string.IsNullOrWhiteSpace(configured)
                ? Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ElevateFitness", "profile-pictures")
                : configured;
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            Directory.CreateDirectory(_root);

            var extension = Path.GetExtension(file.FileName);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine(_root, fileName);

            await using var stream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/uploads/profile-pictures/{fileName}";
        }
    }
}
