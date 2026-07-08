using BuildingBlocks.Shared.Results;
using ProfileService.BuildingBlocks.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

        public async Task<Result<bool>> UpdateEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // Forward the caller's bearer token so AuthService authenticates as the same user
            // (it derives the user id from the token, not from anything we send).
            var token = _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/auth/update-email")
            {
                Content = JsonContent.Create(new { NewEmail = email })
            };

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
                return true;

            if (response.StatusCode == HttpStatusCode.Conflict)
                return Error.Conflict("AUTH_EMAIL_EXISTS", "This email is already in use.");

            return Error.Failure("AUTH_EMAIL_UPDATE_FAILED", "Could not update the email via AuthService.");
        }
    }
}
