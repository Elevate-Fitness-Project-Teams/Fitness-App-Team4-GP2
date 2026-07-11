using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Feature.GetWorkOutDetails.Dtos__ViewModels;

namespace WorkoutService.Feature.SharedFeaturs.Queries.GetWorkOutById
{
    public class WorkoutQueryDto
    {
        public int WorkoutId { get; set; }

        public string PlanId { get; set; } = null!;

        public string WorkoutName { get; set; } = null!;

        public WorkOutCategory WorkoutCategory { get; set; }

        public int DurationInMinutes { get; set; }

        public WorkOutDiffeculty WorkoutDifficulty { get; set; }

        public List<WorkoutQueryExerciseDto> Exercises { get; set; } = [];
    }

    public class WorkoutQueryExerciseDto
    {
        public int ExerciseId { get; set; }

        public string ExerciseName { get; set; } = null!;

        public string TargetMuscles { get; set; } = null!;

        public string ExerciseEquipment { get; set; } = null!;

        public string ExerciseDifficulty { get; set; } = null!;

        public string ExerciseDescription { get; set; } = null!;

        public string? ExerciseVideoUrl { get; set; }

        public int SetsDefault { get; set; }

        public int RepsDefault { get; set; }

        public int RestTimeInSeconds { get; set; }
        public int OrderIndex { get; set; }


    }
}
