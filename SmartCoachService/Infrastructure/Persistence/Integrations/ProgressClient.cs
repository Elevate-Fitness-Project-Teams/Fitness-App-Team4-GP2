using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class ProgressClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProgressClient> _logger;

        public ProgressClient(HttpClient httpClient, ILogger<ProgressClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Null until the user has logged activity; throws
        // DownstreamUnavailableException if the service is down.
        public Task<ProgressStatsResponse?> GetProgressStatsAsync(Guid userId, CancellationToken cancellationToken) =>
            DownstreamHttp.GetOrNullAsync<ProgressStatsResponse>(
                _httpClient, $"/api/v1/Progress/stats/{userId}", _logger, cancellationToken);
    }
}
