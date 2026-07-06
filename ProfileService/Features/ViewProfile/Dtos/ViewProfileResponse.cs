namespace ProfileService.Features.ViewProfile.Dtos
{
    public class ViewProfileResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public FceProfileDto? FceProfile { get; set; } 
        public string? ProfilePictureUrl { get; set; }
        public ProgressStatisticsDto? ProgressStatistics  { get; set; }

    }
}
