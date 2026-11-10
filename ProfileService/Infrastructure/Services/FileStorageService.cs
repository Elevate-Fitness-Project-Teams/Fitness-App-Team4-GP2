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
            if (!string.IsNullOrWhiteSpace(configured))
                return Path.GetFullPath(configured);

            // LocalApplicationData resolves to an empty string in environments without a
            // user profile / HOME (e.g. a Linux container), which would make the combined
            // path relative — and PhysicalFileProvider rejects relative roots. Fall back to
            // the application base directory, which is always absolute.
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrWhiteSpace(baseDir))
                baseDir = AppContext.BaseDirectory;

            return Path.Combine(baseDir, "ElevateFitness", "profile-pictures");
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
