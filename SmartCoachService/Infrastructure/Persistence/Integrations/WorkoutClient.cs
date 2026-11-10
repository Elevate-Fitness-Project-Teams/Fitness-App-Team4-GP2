using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class WorkoutClient
    {
        private const string Path = "/api/v1/WorkOut/GetWorkOuts?page=1&pageSize=3";

        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkoutClient> _logger;

        public WorkoutClient(HttpClient httpClient, ILogger<WorkoutClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET /api/v1/WorkOut/GetWorkOuts - top few workouts to surface on the home feed.
        // Null when the catalog is empty; throws DownstreamUnavailableException if the
        // service is down.
        public async Task<IReadOnlyList<WorkOutItemResponse>?> GetWorkOutsAsync(CancellationToken cancellationToken) =>
            await DownstreamHttp.GetOrNullAsync<List<WorkOutItemResponse>>(
                _httpClient, Path, _logger, cancellationToken);
    }
}
