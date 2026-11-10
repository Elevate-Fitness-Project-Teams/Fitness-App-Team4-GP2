using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class NutritionClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NutritionClient> _logger;

        public NutritionClient(HttpClient httpClient, ILogger<NutritionClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET /api/v1/nutrition/recommendations/{userId} - a small page is enough for the
        // home feed. Null when the user's FCE metrics have not been calculated yet;
        // throws DownstreamUnavailableException if the service is down.
        public Task<MealRecommendationsResponse?> GetMealRecommendationsAsync(Guid userId, CancellationToken cancellationToken) =>
            DownstreamHttp.GetOrNullAsync<MealRecommendationsResponse>(
                _httpClient, $"/api/v1/nutrition/recommendations/{userId}?page=1&pageSize=3", _logger, cancellationToken);
    }
}
