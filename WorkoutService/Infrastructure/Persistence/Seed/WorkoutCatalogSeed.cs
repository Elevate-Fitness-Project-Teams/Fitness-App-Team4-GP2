using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Infrastructure.Persistence.Seed
{
    /// <summary>
    /// Reference data for the workout catalog. Plans, workouts and exercises are
    /// global (not per-user), so an empty table means every caller gets a 404 -
    /// including the SmartCoach home feed. Applied through HasData so the rows ship
    /// with the migration and land identically on a fresh dev box, CI and Docker.
    ///
    /// Keys are fixed literals on purpose: HasData needs stable primary keys or every
    /// subsequent migration would delete and reinsert the whole catalog.
    /// </summary>
    public static class WorkoutCatalogSeed
    {
        public static IEnumerable<Exercise> GetExercises() => new[]
        {
            new Exercise { ExerciseId = 1, Name = "Push-Up", TargetMuscles = "Chest, Triceps, Shoulders", Equipment = "Bodyweight", Difficulty = "beginner", Description = "Lower your chest to the floor with elbows tucked at roughly 45 degrees, then press back up while keeping a straight line from head to heels." },
            new Exercise { ExerciseId = 2, Name = "Bench Press", TargetMuscles = "Chest, Triceps", Equipment = "Barbell, Bench", Difficulty = "intermediate", Description = "Lower the bar to mid-chest under control, then press it back to lockout keeping shoulder blades retracted against the bench." },
            new Exercise { ExerciseId = 3, Name = "Incline Dumbbell Press", TargetMuscles = "Upper Chest, Shoulders", Equipment = "Dumbbells, Incline Bench", Difficulty = "intermediate", Description = "On a 30-45 degree incline, press the dumbbells from chest level to overhead without letting them clash at the top." },
            new Exercise { ExerciseId = 4, Name = "Cable Chest Fly", TargetMuscles = "Chest", Equipment = "Cable Machine", Difficulty = "beginner", Description = "With a slight bend in the elbows, bring both handles together in front of the chest and squeeze before returning slowly." },
            new Exercise { ExerciseId = 5, Name = "Pull-Up", TargetMuscles = "Lats, Biceps", Equipment = "Pull-Up Bar", Difficulty = "advanced", Description = "Hang with an overhand grip and pull until your chin clears the bar, driving the elbows down and back." },
            new Exercise { ExerciseId = 6, Name = "Bent-Over Row", TargetMuscles = "Lats, Rhomboids, Traps", Equipment = "Barbell", Difficulty = "intermediate", Description = "Hinge to about 45 degrees with a flat back and row the bar to your lower ribs, pausing briefly at the top." },
            new Exercise { ExerciseId = 7, Name = "Lat Pulldown", TargetMuscles = "Lats, Biceps", Equipment = "Cable Machine", Difficulty = "beginner", Description = "Pull the bar to your upper chest with the torso nearly upright, then control the bar back to full stretch." },
            new Exercise { ExerciseId = 8, Name = "Seated Cable Row", TargetMuscles = "Mid-Back, Lats", Equipment = "Cable Machine", Difficulty = "beginner", Description = "Keeping the chest tall, draw the handle to your abdomen and squeeze the shoulder blades together." },
            new Exercise { ExerciseId = 9, Name = "Overhead Press", TargetMuscles = "Shoulders, Triceps", Equipment = "Barbell", Difficulty = "intermediate", Description = "Press the bar from the front rack to overhead, moving your head back out of the path and finishing with biceps by the ears." },
            new Exercise { ExerciseId = 10, Name = "Lateral Raise", TargetMuscles = "Side Delts", Equipment = "Dumbbells", Difficulty = "beginner", Description = "Raise the dumbbells out to shoulder height with soft elbows, leading with the elbows rather than the hands." },
            new Exercise { ExerciseId = 11, Name = "Face Pull", TargetMuscles = "Rear Delts, Upper Back", Equipment = "Cable Machine, Rope", Difficulty = "beginner", Description = "Pull the rope toward your forehead while externally rotating the shoulders, finishing with hands beside the ears." },
            new Exercise { ExerciseId = 12, Name = "Bicep Curl", TargetMuscles = "Biceps", Equipment = "Dumbbells", Difficulty = "beginner", Description = "Curl the weight with the elbows pinned to your sides, avoiding any swing from the lower back." },
            new Exercise { ExerciseId = 13, Name = "Triceps Dip", TargetMuscles = "Triceps, Lower Chest", Equipment = "Parallel Bars", Difficulty = "intermediate", Description = "Lower until the upper arms are roughly parallel to the floor, then press back to lockout keeping the torso fairly upright." },
            new Exercise { ExerciseId = 14, Name = "Triceps Rope Pushdown", TargetMuscles = "Triceps", Equipment = "Cable Machine, Rope", Difficulty = "beginner", Description = "Extend the elbows fully and separate the rope ends at the bottom, then return under control without flaring the elbows." },
            new Exercise { ExerciseId = 15, Name = "Back Squat", TargetMuscles = "Quads, Glutes", Equipment = "Barbell, Squat Rack", Difficulty = "intermediate", Description = "Descend until the hips drop below the knees while keeping the chest up and knees tracking over the toes, then drive up through mid-foot." },
            new Exercise { ExerciseId = 16, Name = "Romanian Deadlift", TargetMuscles = "Hamstrings, Glutes", Equipment = "Barbell", Difficulty = "advanced", Description = "Push the hips back with a flat back until you feel a hamstring stretch, then drive the hips forward to stand tall." },
            new Exercise { ExerciseId = 17, Name = "Walking Lunge", TargetMuscles = "Quads, Glutes", Equipment = "Dumbbells", Difficulty = "beginner", Description = "Step forward and lower the back knee toward the floor, then push through the front heel into the next stride." },
            new Exercise { ExerciseId = 18, Name = "Leg Press", TargetMuscles = "Quads, Glutes", Equipment = "Leg Press Machine", Difficulty = "beginner", Description = "Lower the sled until the knees reach about 90 degrees, then press back without locking the knees hard at the top." },
            new Exercise { ExerciseId = 19, Name = "Plank", TargetMuscles = "Core", Equipment = "Bodyweight", Difficulty = "beginner", Description = "Hold a straight line from head to heels on your forearms, bracing the abs and squeezing the glutes throughout." },
            new Exercise { ExerciseId = 20, Name = "Hanging Leg Raise", TargetMuscles = "Lower Abs, Hip Flexors", Equipment = "Pull-Up Bar", Difficulty = "advanced", Description = "Hanging from the bar, raise the legs to hip height or above without swinging, then lower slowly." },
            new Exercise { ExerciseId = 21, Name = "Russian Twist", TargetMuscles = "Obliques", Equipment = "Medicine Ball", Difficulty = "beginner", Description = "Sit leaning back at about 45 degrees and rotate the torso side to side, tapping the weight beside each hip." },
            new Exercise { ExerciseId = 22, Name = "Dead Bug", TargetMuscles = "Core", Equipment = "Bodyweight", Difficulty = "beginner", Description = "Lying on your back, extend the opposite arm and leg while pressing the lower back flat into the floor." },
            new Exercise { ExerciseId = 23, Name = "Burpee", TargetMuscles = "Full Body", Equipment = "Bodyweight", Difficulty = "intermediate", Description = "Drop to a push-up, jump the feet back in, then explode into a vertical jump and repeat without pausing." },
            new Exercise { ExerciseId = 24, Name = "Mountain Climber", TargetMuscles = "Core, Shoulders", Equipment = "Bodyweight", Difficulty = "beginner", Description = "From a high plank, drive the knees toward the chest in an alternating rhythm while keeping the hips level." },
            new Exercise { ExerciseId = 25, Name = "Kettlebell Swing", TargetMuscles = "Glutes, Hamstrings, Core", Equipment = "Kettlebell", Difficulty = "intermediate", Description = "Hinge at the hips and snap them forward to float the bell to chest height; the arms only guide, they do not lift." },
        };

        public static IEnumerable<WorkoutPlan> GetWorkoutPlans() => new[]
        {
            new WorkoutPlan { PlanId = "plan-foundation", Name = "Foundation Full Body", Description = "A three-day full-body introduction that teaches the core movement patterns before adding load or volume.", Goal = "GetFitter", Status = "Active", Difficulty = WorkOutPlanDifficulty.beginner },
            new WorkoutPlan { PlanId = "plan-upper-strength", Name = "Upper Body Strength", Description = "A push/pull split focused on building pressing and rowing strength across the chest, back, shoulders and arms.", Goal = "GainWeight", Status = "Active", Difficulty = WorkOutPlanDifficulty.intermediate },
            new WorkoutPlan { PlanId = "plan-lower-power", Name = "Lower Body Power", Description = "Heavy compound lower-body work built around the squat and hip hinge for athletes with a solid technical base.", Goal = "GainWeight", Status = "Active", Difficulty = WorkOutPlanDifficulty.advanced },
            new WorkoutPlan { PlanId = "plan-core-conditioning", Name = "Core & Conditioning", Description = "Short, low-equipment sessions that build trunk stability and general work capacity.", Goal = "GetFitter", Status = "Active", Difficulty = WorkOutPlanDifficulty.beginner },
            new WorkoutPlan { PlanId = "plan-fat-burn", Name = "Fat Burn Circuit", Description = "High-density circuits that keep the heart rate elevated to maximise calorie burn in a short session.", Goal = "LoseWeight", Status = "Active", Difficulty = WorkOutPlanDifficulty.intermediate },
        };

        public static IEnumerable<Workout> GetWorkouts() => new[]
        {
            new Workout { WorkoutId = 1, PlanId = "plan-core-conditioning", Name = "Core Crusher", Category = WorkOutCategory.stomach, DurationInMinutes = 20, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 2, PlanId = "plan-fat-burn", Name = "HIIT Fat Burn", Category = WorkOutCategory.fullBody, DurationInMinutes = 25, Difficulty = WorkOutDiffeculty.intermediate },
            new Workout { WorkoutId = 3, PlanId = "plan-foundation", Name = "Beginner Back Basics", Category = WorkOutCategory.back, DurationInMinutes = 25, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 4, PlanId = "plan-foundation", Name = "Full Body Kickstart", Category = WorkOutCategory.fullBody, DurationInMinutes = 30, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 5, PlanId = "plan-upper-strength", Name = "Shoulder Sculpt", Category = WorkOutCategory.shoulders, DurationInMinutes = 30, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 6, PlanId = "plan-upper-strength", Name = "Arms Blast", Category = WorkOutCategory.arms, DurationInMinutes = 30, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 7, PlanId = "plan-foundation", Name = "Lower Body Starter", Category = WorkOutCategory.legs, DurationInMinutes = 35, Difficulty = WorkOutDiffeculty.beginner },
            new Workout { WorkoutId = 8, PlanId = "plan-upper-strength", Name = "Chest & Triceps Burnout", Category = WorkOutCategory.chest, DurationInMinutes = 35, Difficulty = WorkOutDiffeculty.advanced },
            new Workout { WorkoutId = 9, PlanId = "plan-upper-strength", Name = "Chest Builder", Category = WorkOutCategory.chest, DurationInMinutes = 40, Difficulty = WorkOutDiffeculty.intermediate },
            new Workout { WorkoutId = 10, PlanId = "plan-upper-strength", Name = "Back Attack", Category = WorkOutCategory.back, DurationInMinutes = 40, Difficulty = WorkOutDiffeculty.intermediate },
            new Workout { WorkoutId = 11, PlanId = "plan-foundation", Name = "Full Body Strength Circuit", Category = WorkOutCategory.fullBody, DurationInMinutes = 45, Difficulty = WorkOutDiffeculty.intermediate },
            new Workout { WorkoutId = 12, PlanId = "plan-lower-power", Name = "Leg Day Power", Category = WorkOutCategory.legs, DurationInMinutes = 50, Difficulty = WorkOutDiffeculty.advanced },
        };

        public static IEnumerable<WorkoutExercise> GetWorkoutExercises() => new[]
        {
            // 1 - Core Crusher
            new WorkoutExercise { Id = 1, WorkoutId = 1, ExerciseId = 19, SetsDefault = 3, RepsDefault = 45, RestTimeInSeconds = 45, OrderIndex = 1 },
            new WorkoutExercise { Id = 2, WorkoutId = 1, ExerciseId = 22, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 45, OrderIndex = 2 },
            new WorkoutExercise { Id = 3, WorkoutId = 1, ExerciseId = 21, SetsDefault = 3, RepsDefault = 20, RestTimeInSeconds = 45, OrderIndex = 3 },
            new WorkoutExercise { Id = 4, WorkoutId = 1, ExerciseId = 24, SetsDefault = 3, RepsDefault = 30, RestTimeInSeconds = 60, OrderIndex = 4 },

            // 2 - HIIT Fat Burn
            new WorkoutExercise { Id = 5, WorkoutId = 2, ExerciseId = 23, SetsDefault = 4, RepsDefault = 12, RestTimeInSeconds = 30, OrderIndex = 1 },
            new WorkoutExercise { Id = 6, WorkoutId = 2, ExerciseId = 25, SetsDefault = 4, RepsDefault = 20, RestTimeInSeconds = 30, OrderIndex = 2 },
            new WorkoutExercise { Id = 7, WorkoutId = 2, ExerciseId = 24, SetsDefault = 4, RepsDefault = 40, RestTimeInSeconds = 30, OrderIndex = 3 },
            new WorkoutExercise { Id = 8, WorkoutId = 2, ExerciseId = 1, SetsDefault = 4, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 4 },

            // 3 - Beginner Back Basics
            new WorkoutExercise { Id = 9, WorkoutId = 3, ExerciseId = 7, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 1 },
            new WorkoutExercise { Id = 10, WorkoutId = 3, ExerciseId = 8, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 2 },
            new WorkoutExercise { Id = 11, WorkoutId = 3, ExerciseId = 11, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 3 },

            // 4 - Full Body Kickstart
            new WorkoutExercise { Id = 12, WorkoutId = 4, ExerciseId = 18, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 75, OrderIndex = 1 },
            new WorkoutExercise { Id = 13, WorkoutId = 4, ExerciseId = 1, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 60, OrderIndex = 2 },
            new WorkoutExercise { Id = 14, WorkoutId = 4, ExerciseId = 7, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 3 },
            new WorkoutExercise { Id = 15, WorkoutId = 4, ExerciseId = 19, SetsDefault = 3, RepsDefault = 30, RestTimeInSeconds = 45, OrderIndex = 4 },

            // 5 - Shoulder Sculpt
            new WorkoutExercise { Id = 16, WorkoutId = 5, ExerciseId = 9, SetsDefault = 4, RepsDefault = 8, RestTimeInSeconds = 90, OrderIndex = 1 },
            new WorkoutExercise { Id = 17, WorkoutId = 5, ExerciseId = 10, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 2 },
            new WorkoutExercise { Id = 18, WorkoutId = 5, ExerciseId = 11, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 3 },

            // 6 - Arms Blast
            new WorkoutExercise { Id = 19, WorkoutId = 6, ExerciseId = 12, SetsDefault = 4, RepsDefault = 12, RestTimeInSeconds = 45, OrderIndex = 1 },
            new WorkoutExercise { Id = 20, WorkoutId = 6, ExerciseId = 14, SetsDefault = 4, RepsDefault = 12, RestTimeInSeconds = 45, OrderIndex = 2 },
            new WorkoutExercise { Id = 21, WorkoutId = 6, ExerciseId = 13, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 60, OrderIndex = 3 },

            // 7 - Lower Body Starter
            new WorkoutExercise { Id = 22, WorkoutId = 7, ExerciseId = 18, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 75, OrderIndex = 1 },
            new WorkoutExercise { Id = 23, WorkoutId = 7, ExerciseId = 17, SetsDefault = 3, RepsDefault = 20, RestTimeInSeconds = 60, OrderIndex = 2 },
            new WorkoutExercise { Id = 24, WorkoutId = 7, ExerciseId = 19, SetsDefault = 3, RepsDefault = 30, RestTimeInSeconds = 45, OrderIndex = 3 },

            // 8 - Chest & Triceps Burnout
            new WorkoutExercise { Id = 25, WorkoutId = 8, ExerciseId = 3, SetsDefault = 4, RepsDefault = 10, RestTimeInSeconds = 75, OrderIndex = 1 },
            new WorkoutExercise { Id = 26, WorkoutId = 8, ExerciseId = 4, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 2 },
            new WorkoutExercise { Id = 27, WorkoutId = 8, ExerciseId = 13, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 3 },
            new WorkoutExercise { Id = 28, WorkoutId = 8, ExerciseId = 14, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 4 },

            // 9 - Chest Builder
            new WorkoutExercise { Id = 29, WorkoutId = 9, ExerciseId = 2, SetsDefault = 4, RepsDefault = 8, RestTimeInSeconds = 120, OrderIndex = 1 },
            new WorkoutExercise { Id = 30, WorkoutId = 9, ExerciseId = 3, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 90, OrderIndex = 2 },
            new WorkoutExercise { Id = 31, WorkoutId = 9, ExerciseId = 4, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 3 },
            new WorkoutExercise { Id = 32, WorkoutId = 9, ExerciseId = 1, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 60, OrderIndex = 4 },

            // 10 - Back Attack
            new WorkoutExercise { Id = 33, WorkoutId = 10, ExerciseId = 5, SetsDefault = 4, RepsDefault = 6, RestTimeInSeconds = 120, OrderIndex = 1 },
            new WorkoutExercise { Id = 34, WorkoutId = 10, ExerciseId = 6, SetsDefault = 4, RepsDefault = 10, RestTimeInSeconds = 90, OrderIndex = 2 },
            new WorkoutExercise { Id = 35, WorkoutId = 10, ExerciseId = 8, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 3 },
            new WorkoutExercise { Id = 36, WorkoutId = 10, ExerciseId = 11, SetsDefault = 3, RepsDefault = 15, RestTimeInSeconds = 45, OrderIndex = 4 },

            // 11 - Full Body Strength Circuit
            new WorkoutExercise { Id = 37, WorkoutId = 11, ExerciseId = 15, SetsDefault = 4, RepsDefault = 8, RestTimeInSeconds = 120, OrderIndex = 1 },
            new WorkoutExercise { Id = 38, WorkoutId = 11, ExerciseId = 2, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 90, OrderIndex = 2 },
            new WorkoutExercise { Id = 39, WorkoutId = 11, ExerciseId = 6, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 90, OrderIndex = 3 },
            new WorkoutExercise { Id = 40, WorkoutId = 11, ExerciseId = 9, SetsDefault = 3, RepsDefault = 10, RestTimeInSeconds = 90, OrderIndex = 4 },
            new WorkoutExercise { Id = 41, WorkoutId = 11, ExerciseId = 19, SetsDefault = 3, RepsDefault = 45, RestTimeInSeconds = 45, OrderIndex = 5 },

            // 12 - Leg Day Power
            new WorkoutExercise { Id = 42, WorkoutId = 12, ExerciseId = 15, SetsDefault = 5, RepsDefault = 5, RestTimeInSeconds = 180, OrderIndex = 1 },
            new WorkoutExercise { Id = 43, WorkoutId = 12, ExerciseId = 16, SetsDefault = 4, RepsDefault = 8, RestTimeInSeconds = 120, OrderIndex = 2 },
            new WorkoutExercise { Id = 44, WorkoutId = 12, ExerciseId = 17, SetsDefault = 3, RepsDefault = 20, RestTimeInSeconds = 90, OrderIndex = 3 },
            new WorkoutExercise { Id = 45, WorkoutId = 12, ExerciseId = 18, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 90, OrderIndex = 4 },
            new WorkoutExercise { Id = 46, WorkoutId = 12, ExerciseId = 20, SetsDefault = 3, RepsDefault = 12, RestTimeInSeconds = 60, OrderIndex = 5 },
        };
    }
}
