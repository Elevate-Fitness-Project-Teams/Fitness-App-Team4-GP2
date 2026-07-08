using ProfileService.Features.ChangePassword.Dtos;
using BuildingBlocks.Shared.Results;
using ProfileService.BuildingBlocks.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;

namespace ProfileService.Infrastructure.Services
{
    public class AuthClient : IAuthClient

    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthClient(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var token = _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, "/api/v1/auth/change-password")
            {
                Content = JsonContent.Create(new {
                    currentPassword = request.CurrentPassword,
                    newPassword = request.NewPassword,
                    confirmPassword = request.ConfirmPassword
                })
            };

            if (!string.IsNullOrWhiteSpace(token))
                httpRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

            var response = await _httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
                return true;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return Error.InvalidCredentials("AUTH_INVALID_CREDENTIALS", "Current password is incorrect.");
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return Error.Validation("AUTH_PASSWORD_MISMATCH", "The new password is invalid.");

            return Error.Failure("AUTH_PASSWORD_CHANGE_FAILED", "Could not change the password via AuthService.");
        }

        public async Task<Result<bool>> UpdateUserInfoAsync(string firstName, string lastName, string phoneNumber, string email, CancellationToken cancellationToken = default)
        {
            // Forward the caller's bearer token so AuthService authenticates as the same user
            // (it derives the user id from the token, not from anything we send).
            var token = _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, "/api/v1/auth/update-user-info")
            {
                Content = JsonContent.Create(new
                {
                    firstName,
                    lastName,
                    phoneNumber,
                    email
                })
            };

            if (!string.IsNullOrWhiteSpace(token))
                httpRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
                return true;

            if (response.StatusCode == HttpStatusCode.Conflict)
                return Error.Conflict("AUTH_EMAIL_EXISTS", "This email is already in use.");

            return Error.Failure("AUTH_USER_UPDATE_FAILED", "Could not update user information via AuthService.");
        }
    }
}
