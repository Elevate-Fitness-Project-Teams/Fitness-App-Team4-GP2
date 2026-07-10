using BuildingBlocks.Shared.Responses;
using SmartCoachService.Infrastructure.Persistence.Integrations.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    public class ProgressClient
    {
        private readonly HttpClient _httpClient;

        public ProgressClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProgressStatsResponse?> GetProgressStatsAsync(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Progress/stats/{userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;
            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<ProgressStatsResponse>>(cancellationToken);
            return apiResult?.Data;
        }

        
    }
}
