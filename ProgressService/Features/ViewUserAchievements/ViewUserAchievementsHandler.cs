using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Features.ViewUserAchievements
{
    public class ViewUserAchievementsHandler : IRequestHandler<ViewUserAchievementsQuery, Result<UserAchievementsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewUserAchievementsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserAchievementsResponse>> Handle(ViewUserAchievementsQuery request, CancellationToken cancellationToken)
        {
            var achievements =await  _unitOfWork.GetRepository<UserAchievement,int>().GetAll(a => a.UserId == request.UserId)
                .Select(a => new UserAchievementsResponse.AchievementEntry
                {
                    Name = a.Achievement.Name,
                    Description = a.Achievement.Description,
                    IconUrl = a.Achievement.IconUrl,
                    EarnedAt = a.EarnedAt
                }).ToListAsync();

            return new UserAchievementsResponse
            {
                Achievements = achievements
            };

            
        }
    }
}
