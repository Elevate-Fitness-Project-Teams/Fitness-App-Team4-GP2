namespace AuthService.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Sends transactional emails (OTP codes, notifications, etc.).
    /// </summary>
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }
}
