using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.Login.Dtos;
using AuthService.Infrastructure.Services.Interfaces;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Features.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IJwtService _jwtService;
        private readonly ILoginAttemptRepository _attemptRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IGenericRepository<Domain.Entities.RefreshToken> _refreshRepository;

        public LoginCommandHandler(
                    UserManager<ApplicationUser> user,
                    IJwtService jwtService,
                    ILoginAttemptRepository attemptRepository,
                    IHttpContextAccessor httpContext,
                    IGenericRepository<Domain.Entities.RefreshToken> refreshRepository)
        {
            _user = user;
            _jwtService = jwtService;
            _attemptRepository = attemptRepository;
            _httpContext = httpContext;
            _refreshRepository = refreshRepository;
        }
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var ipAddress = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var user = await _user.FindByEmailAsync(request.email);
            if (user == null)
            {
                await _attemptRepository.AddAsync(new LoginAttempt
                {
                    Email = request.email,
                    AttemptedAt = DateTime.UtcNow,
                    IsSuccess = false,
                    IpAddress = ipAddress?? string.Empty
                });
                await _attemptRepository.SaveChangesAsync();

                return Error.InvalidCredentials("AUTH_INVALID_CREDENTIALS", "Invalid email or password.");
            }

            if (await _user.IsLockedOutAsync(user))
                return Error.Locked("AUTH_ACCOUNT_LOCKED", "Account is locked due to too many failed attempts. Try again later.");


            var validPass = await _user.CheckPasswordAsync(user, request.password);
            if (!validPass)
            {
                await _user.AccessFailedAsync(user);
                await _attemptRepository.AddAsync(new LoginAttempt
                {
                    Email = request.email,
                    AttemptedAt = DateTime.UtcNow,
                    IsSuccess = false,
                    IpAddress = ipAddress ?? string.Empty
                });
                await _attemptRepository.SaveChangesAsync();

                if (await _user.IsLockedOutAsync(user))
                    return Error.Locked("AUTH_ACCOUNT_LOCKED", "Account is locked due to too many failed attempts. Try again later.");

                return Error.InvalidCredentials("AUTH_INVALID_CREDENTIALS", "Invalid email or password.");
            }

            await _user.ResetAccessFailedCountAsync(user);
            await _attemptRepository.AddAsync(
                    new LoginAttempt
                    {
                        Email = user.Email!,
                        AttemptedAt = DateTime.UtcNow,
                        IsSuccess = true,
                        IpAddress = ipAddress ?? string.Empty
                    });

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshTokenValue = _jwtService.GenerateRefreshToken();

            await _refreshRepository.AddAsync(new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _refreshRepository.SaveChangesAsync();

            return new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshTokenValue,
                ProfileCompleted = !user.RequiresProfileCompletion,
                IsPremium = user.IsPremium
            };

        }
    }
}
