namespace WorkoutService.Feature.Browse_Exercise_Library.Dtos__ViewModels
{
    public class ExerciseDto
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public string Description { get; set; } = null!;    
    }
}
