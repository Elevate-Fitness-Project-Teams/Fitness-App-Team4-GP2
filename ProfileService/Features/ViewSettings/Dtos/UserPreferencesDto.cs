namespace ProfileService.Features.ViewSettings.Dtos
{
    public class UserPreferencesDto
    {
        public string Language { get; set; } = default!;
        public string Theme { get; set; } = default!;
        public string WeightUnit { get; set; } = default!;
        public string HeightUnit { get; set; } = default!;
        public string DistanceUnit { get; set; } = default!;
    }
}
