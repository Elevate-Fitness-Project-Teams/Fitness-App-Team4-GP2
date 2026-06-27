namespace ProgressService.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public CriteriaType CriteriaType { get; set; } = CriteriaType.WORKOUT_COUNT; // 'WORKOUT_COUNT', 'STREAK_DAYS', 'CALORIES_BURNED', 'WORKOUTS_IN_WEEK'
        public int CriteriaValue { get; set; } = 0;

    }
}
