using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Features.RefreshToken.Dtos;
using AuthService.Infrastructure.Persistence.Repositories;
using AuthService.Infrastructure.Services.Interfaces;
using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Features.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IGenericRepository<ApplicationUser> _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokens;

        public RefreshTokenHandler(IGenericRepository<ApplicationUser> userRepo,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokens)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _refreshTokens = refreshTokens;
        }
        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokens.GetByTokenAsync(request.refreshToken);

            if (refreshToken == null || refreshToken.RevokedAt != null)
                return Error.InvalidCredentials(" AUTH_TOKEN_INVALID");

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
                return Error.InvalidCredentials("AUTH_TOKEN_EXPIRED");

            var user = await _userRepo.GetAllAsync(x => x.Id == refreshToken.UserId).FirstOrDefaultAsync();

            if (user == null)
                return Error.NotFound("USER_NOT_FOUND");

            var activeTokens = await _refreshTokens
                            .GetAllAsync(t => t.UserId == user.Id && t.RevokedAt == null)
                            .ToListAsync(cancellationToken);

            foreach (var token in activeTokens)
                token.RevokedAt = DateTime.UtcNow;

            var accessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokens.AddAsync(new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
            });

            await _refreshTokens.SaveChangesAsync();

            return new RefreshTokenResponse(accessToken, newRefreshToken);
        }
    }
}
