using BuildingBlocks.Shared.Responses;

namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    /// <summary>
    /// Thrown when a downstream service could not answer at all - unreachable, timed
    /// out, or returned 5xx. The aggregator turns this into a single 503 for the whole
    /// feed, which is the one case where a partial feed would be misleading rather
    /// than merely incomplete.
    /// </summary>
    public class DownstreamUnavailableException : Exception
    {
        public DownstreamUnavailableException(string uri, string reason, Exception? inner = null)
            : base($"Downstream call to {uri} failed: {reason}", inner)
        {
            Uri = uri;
        }

        public string Uri { get; }
    }

    /// <summary>
    /// Shared GET plumbing for the five downstream clients. Collapses the two failure
    /// modes onto plain C# semantics so the clients can return a simple <c>T?</c>:
    ///
    ///  - null  -> the service answered fine but has nothing for this user yet
    ///             (4xx, or a 2xx envelope with no payload). Renders as an absent
    ///             section; it must not fail the feed.
    ///  - throw -> the service is genuinely down (transport failure or 5xx).
    /// </summary>
    internal static class DownstreamHttp
    {
        public static async Task<T?> GetOrNullAsync<T>(
            HttpClient httpClient,
            string path,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var uri = httpClient.BaseAddress is null
                ? path
                : new Uri(httpClient.BaseAddress, path).ToString();

            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(path, cancellationToken);
            }
            catch (Exception ex) when ((ex is HttpRequestException or TaskCanceledException)
                                       && !cancellationToken.IsCancellationRequested)
            {
                logger.LogError(ex, "GET {Uri} failed.", uri);
                throw new DownstreamUnavailableException(uri, "unreachable or timed out", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                var status = (int)response.StatusCode;
                var body = await response.Content.ReadAsStringAsync(cancellationToken);

                if (status >= 500)
                {
                    logger.LogError("GET {Uri} returned {StatusCode}: {Body}", uri, status, body);
                    throw new DownstreamUnavailableException(uri, $"returned {status}");
                }

                // A 4xx here means "healthy service, no data for this user" - a new
                // account, metrics not calculated yet, an empty catalog.
                logger.LogInformation(
                    "GET {Uri} returned {StatusCode}, treated as no data: {Body}", uri, status, body);
                return default;
            }

            var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(cancellationToken);
            if (apiResult is null || apiResult.Data is null)
            {
                logger.LogWarning("GET {Uri} succeeded but carried no data.", uri);
                return default;
            }

            return apiResult.Data;
        }
    }
}
