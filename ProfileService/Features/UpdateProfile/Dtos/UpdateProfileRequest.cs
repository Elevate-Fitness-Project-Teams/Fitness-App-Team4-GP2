namespace ProfileService.Features.UpdateProfile.Dtos
{
    public record UpdateProfileRequest(string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);
}
