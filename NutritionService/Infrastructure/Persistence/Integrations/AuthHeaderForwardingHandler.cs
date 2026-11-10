namespace NutritionService.Infrastructure.Persistence.Integrations
{
    /// <summary>
    /// Forwards the caller's incoming Authorization header onto outgoing service-to-service
    /// requests. FCE's GET /api/v1/Fitness/metrics/{userId} is [Authorize], so without this
    /// FceClient always got a 401 and the handler reported FCE_METRICS_NOT_CALCULATED even
    /// when the metrics existed.
    /// </summary>
    public class AuthHeaderForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHeaderForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization is null)
            {
                var incoming = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
                if (!string.IsNullOrWhiteSpace(incoming))
                    request.Headers.TryAddWithoutValidation("Authorization", incoming);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
