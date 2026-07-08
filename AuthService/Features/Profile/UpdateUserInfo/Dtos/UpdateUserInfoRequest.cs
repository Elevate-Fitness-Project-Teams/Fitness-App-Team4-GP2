namespace AuthService.Features.Profile.UpdateUserInfo.Dtos
{
    public record UpdateUserInfoRequest(string FirstName, string LastName, string PhoneNumber, string Email);
}
