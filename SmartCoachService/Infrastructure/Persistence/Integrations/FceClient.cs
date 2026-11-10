using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class FceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FceClient> _logger;

        public FceClient(HttpClient httpClient, ILogger<FceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Null until the user submits stats and runs /calculate; throws
        // DownstreamUnavailableException if the service is down.
        public Task<FitnessMetricsResponse?> GetMetricsAsync(Guid userId, CancellationToken cancellationToken) =>
            DownstreamHttp.GetOrNullAsync<FitnessMetricsResponse>(
                _httpClient, $"/api/v1/Fitness/metrics/{userId}", _logger, cancellationToken);
    }
}
