using BuildingBlocks.Shared.Responses;
using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class FceClient
    {
        private readonly HttpClient _httpClient;

        public FceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FitnessMetricsResponse?> GetMetricsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Fitness/metrics/{userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<FitnessMetricsResponse>>(cancellationToken);

            return apiResult?.Data;
        }
    }
}

