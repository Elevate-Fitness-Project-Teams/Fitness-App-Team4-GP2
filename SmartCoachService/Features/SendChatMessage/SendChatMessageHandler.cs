using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCoachService.Domain.Contracts;
using SmartCoachService.Domain.Entities;
using SmartCoachService.Features.SendChatMessage.DTOs;
using SmartCoachService.Infrastructure.Persistence.Integrations;
using System.Text.Json;

namespace SmartCoachService.Features.SendChatMessage
{
    public class SendChatMessageHandler : IRequestHandler<SendChatMessageCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FceClient _fceClient;
        private readonly ProgressClient _progressClient;

        public SendChatMessageHandler(IUnitOfWork unitOfWork, FceClient fceClient, ProgressClient progressClient)
        {
            _unitOfWork = unitOfWork;
            _fceClient = fceClient;
            _progressClient = progressClient;
        }
        public Task<Result<string>> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetUserContext(Guid userId, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<RecommendationCache>();
            var cache = await repo.GetOneAsync(x => x.UserId == userId);
            if (cache is not null && cache.ExpiresAt > DateTime.UtcNow)
                return cache.UserContextJson;

            var userProgressStats = await _progressClient.GetProgressStatsAsync(userId, cancellationToken);
            var userFceMetrics = await _fceClient.GetMetricsAsync(userId, cancellationToken);

            var userContext = new UserContextDto
            {
                Bmr = userFceMetrics!.Bmr,
                Tdee = userFceMetrics.Tdee,
                CalorieTarget = userFceMetrics.CalorieTarget,
                TotalWorkouts = userProgressStats!.TotalWorkouts,
                TotalCaloriesBurned = userProgressStats.TotalCaloriesBurned,
                TotalWeightLost = userProgressStats.TotalWeightLost,
                CurrentStreak = userProgressStats.CurrentStreak,
                LongestStreak = userProgressStats.LongestStreak
            };
            var userContextString = JsonSerializer.Serialize(userContext);
            if (cache is null)
            {
                cache = new RecommendationCache
                {
                    UserId = userId,
                    UserContextJson = userContextString,
                    CachedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                };
                 repo.Add(cache);
            }
            else
            {
                cache.UserContextJson = userContextString;
                cache.CachedAt = DateTime.UtcNow;
                cache.ExpiresAt = DateTime.UtcNow.AddHours(1);
            }
            return userContextString;





        }

    }
}
