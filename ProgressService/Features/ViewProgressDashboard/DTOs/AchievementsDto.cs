using ProgressService.Domain.Entities;

namespace ProgressService.Features.ViewProgressDashboard.DTOs
{
    public record AchievementsDto(int AchievementId, string IconUrl, string Name, CriteriaType CriteriaType);
    
}
