namespace ProfileService.BuildingBlocks.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveProfilePictureAsync(IFormFile file);
    }
}
