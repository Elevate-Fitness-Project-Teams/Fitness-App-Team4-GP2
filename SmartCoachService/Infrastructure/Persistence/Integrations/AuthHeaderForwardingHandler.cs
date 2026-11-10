namespace SmartCoachService.Infrastructure.Persistence.Integrations
{
    /// <summary>
    /// Forwards the caller's incoming Authorization header onto outgoing service-to-service
    /// requests, so downstream endpoints that derive the user from the JWT (e.g. the Profile
    /// service's GET /api/v1/profile) receive the same identity.
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
