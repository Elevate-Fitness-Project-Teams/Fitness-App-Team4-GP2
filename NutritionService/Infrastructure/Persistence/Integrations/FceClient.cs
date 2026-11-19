using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.Shared.Responses;
using BuildingBlocks.Shared.Results;
using Microsoft.Extensions.Logging;

namespace NutritionService.Infrastructure.Persistence.Integrations
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

        public async Task<Result<FitnessMetricsResponse>> GetMetricsAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/v1/Fitness/metrics/{userId}", cancellationToken);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result<FitnessMetricsResponse>.Fail(
                        Error.Validation("FCE_METRICS_NOT_CALCULATED", "Fitness metrics have not been calculated for this user."));
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("FCEService responded with status code {StatusCode} for user {UserId}", response.StatusCode, userId);
                    return Result<FitnessMetricsResponse>.Fail(
                        Error.ServiceUnavailable("SRV_SERVICE_UNAVAILABLE", "Downstream FCEService returned an error."));
                }

                var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<FitnessMetricsResponse>>(cancellationToken);
                if (apiResult?.Data is null)
                {
                    return Result<FitnessMetricsResponse>.Fail(
                        Error.Validation("FCE_METRICS_NOT_CALCULATED", "Fitness metrics have not been calculated for this user."));
                }

                return Result<FitnessMetricsResponse>.OK(apiResult.Data);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to connect to FCEService for user {UserId}", userId);
                return Result<FitnessMetricsResponse>.Fail(
                    Error.ServiceUnavailable("SRV_SERVICE_UNAVAILABLE", "Downstream FCEService is unreachable."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error calling FCEService for user {UserId}", userId);
                return Result<FitnessMetricsResponse>.Fail(
                    Error.ServiceUnavailable("SRV_SERVICE_UNAVAILABLE", "Downstream FCEService is unreachable."));
            }
        }
    }
}
