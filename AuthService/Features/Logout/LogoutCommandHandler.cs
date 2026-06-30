using AuthService.BuildingBlocks.Interfaces;
using AuthService.Features.Logout.Dtos;
using AuthService.Infrastructure.Persistence.Repositories;
using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
    {
        private readonly ICurrentUser _user;
        private readonly IRefreshTokenRepository _tokenRepository;

        public LogoutCommandHandler(ICurrentUser user, IRefreshTokenRepository tokenRepository)
        {
            _user = user;
            _tokenRepository = tokenRepository;
        }
        public async Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_user.UserId, out var userId))
                return Error.InvalidCredentials("AUTH_TOKEN_INVALID", "Authentication token is missing or invalid.");

            var tokens = await _tokenRepository.GetActiveTokensAsync(userId);

            if (tokens is {Count: > 0})
            {
                foreach (var token in tokens)
                    token.RevokedAt = DateTime.UtcNow;

                await _tokenRepository.SaveChangesAsync();
            }

            return new LogoutResponse(true);
        }
    }
}
