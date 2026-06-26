using AuthService.BuildingBlocks.Exceptions;
using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.Login.Dtos;
using AuthService.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Features.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
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
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
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

                throw new UnauthorizedException("AUTH_INVALID_CREDENTIALS");
            }

            if (await _user.IsLockedOutAsync(user))
                throw new UserLockedOutException("AUTH_ACCOUNT_LOCKED");


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
                    throw new UserLockedOutException("AUTH_ACCOUNT_LOCKED");
                else throw new UnauthorizedException("AUTH_INVALID_CREDENTIALS");
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
