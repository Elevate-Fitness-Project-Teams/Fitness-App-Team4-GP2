using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class ProfileClient
    {
        private const string Path = "/api/v1/profile";

        private readonly HttpClient _httpClient;
        private readonly ILogger<ProfileClient> _logger;

        public ProfileClient(HttpClient httpClient, ILogger<ProfileClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET /api/v1/profile - the Profile service derives the user from the forwarded JWT
        // (AuthHeaderForwardingHandler), so no userId is passed on the route.
        // Null when the user has no profile yet; throws DownstreamUnavailableException
        // if the service is down.
        public Task<ProfileResponse?> GetProfileAsync(CancellationToken cancellationToken) =>
            DownstreamHttp.GetOrNullAsync<ProfileResponse>(_httpClient, Path, _logger, cancellationToken);
    }
}
