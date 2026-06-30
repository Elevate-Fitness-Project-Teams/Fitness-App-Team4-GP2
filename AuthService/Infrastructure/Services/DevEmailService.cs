using AuthService.Infrastructure.Services.Interfaces;

namespace AuthService.Infrastructure.Services
{
    /// <summary>
    /// Development-only "email" sink: logs the message (including the OTP code) instead of
    /// sending it, so the flow is testable without an SMTP server. Never registered in production.
    /// </summary>
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<DevEmailService> _logger;

        public DevEmailService(ILogger<DevEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning(
                "[DEV EMAIL] To: {To} | Subject: {Subject}\n{Body}",
                to, subject, body);

            return Task.CompletedTask;
        }
    }
}
