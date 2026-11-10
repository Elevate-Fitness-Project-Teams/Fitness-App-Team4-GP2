using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkoutService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedWorkoutCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "ExerciseId", "Description", "Difficulty", "Equipment", "Name", "TargetMuscles", "VideoUrl" },
                values: new object[,]
                {
                    { 1, "Lower your chest to the floor with elbows tucked at roughly 45 degrees, then press back up while keeping a straight line from head to heels.", "beginner", "Bodyweight", "Push-Up", "Chest, Triceps, Shoulders", null },
                    { 2, "Lower the bar to mid-chest under control, then press it back to lockout keeping shoulder blades retracted against the bench.", "intermediate", "Barbell, Bench", "Bench Press", "Chest, Triceps", null },
                    { 3, "On a 30-45 degree incline, press the dumbbells from chest level to overhead without letting them clash at the top.", "intermediate", "Dumbbells, Incline Bench", "Incline Dumbbell Press", "Upper Chest, Shoulders", null },
                    { 4, "With a slight bend in the elbows, bring both handles together in front of the chest and squeeze before returning slowly.", "beginner", "Cable Machine", "Cable Chest Fly", "Chest", null },
                    { 5, "Hang with an overhand grip and pull until your chin clears the bar, driving the elbows down and back.", "advanced", "Pull-Up Bar", "Pull-Up", "Lats, Biceps", null },
                    { 6, "Hinge to about 45 degrees with a flat back and row the bar to your lower ribs, pausing briefly at the top.", "intermediate", "Barbell", "Bent-Over Row", "Lats, Rhomboids, Traps", null },
                    { 7, "Pull the bar to your upper chest with the torso nearly upright, then control the bar back to full stretch.", "beginner", "Cable Machine", "Lat Pulldown", "Lats, Biceps", null },
                    { 8, "Keeping the chest tall, draw the handle to your abdomen and squeeze the shoulder blades together.", "beginner", "Cable Machine", "Seated Cable Row", "Mid-Back, Lats", null },
                    { 9, "Press the bar from the front rack to overhead, moving your head back out of the path and finishing with biceps by the ears.", "intermediate", "Barbell", "Overhead Press", "Shoulders, Triceps", null },
                    { 10, "Raise the dumbbells out to shoulder height with soft elbows, leading with the elbows rather than the hands.", "beginner", "Dumbbells", "Lateral Raise", "Side Delts", null },
                    { 11, "Pull the rope toward your forehead while externally rotating the shoulders, finishing with hands beside the ears.", "beginner", "Cable Machine, Rope", "Face Pull", "Rear Delts, Upper Back", null },
                    { 12, "Curl the weight with the elbows pinned to your sides, avoiding any swing from the lower back.", "beginner", "Dumbbells", "Bicep Curl", "Biceps", null },
                    { 13, "Lower until the upper arms are roughly parallel to the floor, then press back to lockout keeping the torso fairly upright.", "intermediate", "Parallel Bars", "Triceps Dip", "Triceps, Lower Chest", null },
                    { 14, "Extend the elbows fully and separate the rope ends at the bottom, then return under control without flaring the elbows.", "beginner", "Cable Machine, Rope", "Triceps Rope Pushdown", "Triceps", null },
                    { 15, "Descend until the hips drop below the knees while keeping the chest up and knees tracking over the toes, then drive up through mid-foot.", "intermediate", "Barbell, Squat Rack", "Back Squat", "Quads, Glutes", null },
                    { 16, "Push the hips back with a flat back until you feel a hamstring stretch, then drive the hips forward to stand tall.", "advanced", "Barbell", "Romanian Deadlift", "Hamstrings, Glutes", null },
                    { 17, "Step forward and lower the back knee toward the floor, then push through the front heel into the next stride.", "beginner", "Dumbbells", "Walking Lunge", "Quads, Glutes", null },
                    { 18, "Lower the sled until the knees reach about 90 degrees, then press back without locking the knees hard at the top.", "beginner", "Leg Press Machine", "Leg Press", "Quads, Glutes", null },
                    { 19, "Hold a straight line from head to heels on your forearms, bracing the abs and squeezing the glutes throughout.", "beginner", "Bodyweight", "Plank", "Core", null },
                    { 20, "Hanging from the bar, raise the legs to hip height or above without swinging, then lower slowly.", "advanced", "Pull-Up Bar", "Hanging Leg Raise", "Lower Abs, Hip Flexors", null },
                    { 21, "Sit leaning back at about 45 degrees and rotate the torso side to side, tapping the weight beside each hip.", "beginner", "Medicine Ball", "Russian Twist", "Obliques", null },
                    { 22, "Lying on your back, extend the opposite arm and leg while pressing the lower back flat into the floor.", "beginner", "Bodyweight", "Dead Bug", "Core", null },
                    { 23, "Drop to a push-up, jump the feet back in, then explode into a vertical jump and repeat without pausing.", "intermediate", "Bodyweight", "Burpee", "Full Body", null },
                    { 24, "From a high plank, drive the knees toward the chest in an alternating rhythm while keeping the hips level.", "beginner", "Bodyweight", "Mountain Climber", "Core, Shoulders", null },
                    { 25, "Hinge at the hips and snap them forward to float the bell to chest height; the arms only guide, they do not lift.", "intermediate", "Kettlebell", "Kettlebell Swing", "Glutes, Hamstrings, Core", null }
                });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "PlanId", "Description", "Difficulty", "Goal", "Name", "Status" },
                values: new object[,]
                {
                    { "plan-core-conditioning", "Short, low-equipment sessions that build trunk stability and general work capacity.", 1, "GetFitter", "Core & Conditioning", "Active" },
                    { "plan-fat-burn", "High-density circuits that keep the heart rate elevated to maximise calorie burn in a short session.", 2, "LoseWeight", "Fat Burn Circuit", "Active" },
                    { "plan-foundation", "A three-day full-body introduction that teaches the core movement patterns before adding load or volume.", 1, "GetFitter", "Foundation Full Body", "Active" },
                    { "plan-lower-power", "Heavy compound lower-body work built around the squat and hip hinge for athletes with a solid technical base.", 3, "GainWeight", "Lower Body Power", "Active" },
                    { "plan-upper-strength", "A push/pull split focused on building pressing and rowing strength across the chest, back, shoulders and arms.", 2, "GainWeight", "Upper Body Strength", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Workouts",
                columns: new[] { "WorkoutId", "Category", "Difficulty", "DurationInMinutes", "Name", "PlanId" },
                values: new object[,]
                {
                    { 1, 7, 1, 20, "Core Crusher", "plan-core-conditioning" },
                    { 2, 1, 2, 25, "HIIT Fat Burn", "plan-fat-burn" },
                    { 3, 3, 1, 25, "Beginner Back Basics", "plan-foundation" },
                    { 4, 1, 1, 30, "Full Body Kickstart", "plan-foundation" },
                    { 5, 4, 1, 30, "Shoulder Sculpt", "plan-upper-strength" },
                    { 6, 5, 1, 30, "Arms Blast", "plan-upper-strength" },
                    { 7, 6, 1, 35, "Lower Body Starter", "plan-foundation" },
                    { 8, 2, 3, 35, "Chest & Triceps Burnout", "plan-upper-strength" },
                    { 9, 2, 2, 40, "Chest Builder", "plan-upper-strength" },
                    { 10, 3, 2, 40, "Back Attack", "plan-upper-strength" },
                    { 11, 1, 2, 45, "Full Body Strength Circuit", "plan-foundation" },
                    { 12, 6, 3, 50, "Leg Day Power", "plan-lower-power" }
                });

            migrationBuilder.InsertData(
                table: "WorkoutExercises",
                columns: new[] { "Id", "ExerciseId", "OrderIndex", "RepsDefault", "RestTimeInSeconds", "SetsDefault", "WorkoutId" },
                values: new object[,]
                {
                    { 1, 19, 1, 45, 45, 3, 1 },
                    { 2, 22, 2, 12, 45, 3, 1 },
                    { 3, 21, 3, 20, 45, 3, 1 },
                    { 4, 24, 4, 30, 60, 3, 1 },
                    { 5, 23, 1, 12, 30, 4, 2 },
                    { 6, 25, 2, 20, 30, 4, 2 },
                    { 7, 24, 3, 40, 30, 4, 2 },
                    { 8, 1, 4, 15, 45, 4, 2 },
                    { 9, 7, 1, 12, 60, 3, 3 },
                    { 10, 8, 2, 12, 60, 3, 3 },
                    { 11, 11, 3, 15, 45, 3, 3 },
                    { 12, 18, 1, 12, 75, 3, 4 },
                    { 13, 1, 2, 10, 60, 3, 4 },
                    { 14, 7, 3, 12, 60, 3, 4 },
                    { 15, 19, 4, 30, 45, 3, 4 },
                    { 16, 9, 1, 8, 90, 4, 5 },
                    { 17, 10, 2, 15, 45, 3, 5 },
                    { 18, 11, 3, 15, 45, 3, 5 },
                    { 19, 12, 1, 12, 45, 4, 6 },
                    { 20, 14, 2, 12, 45, 4, 6 },
                    { 21, 13, 3, 10, 60, 3, 6 },
                    { 22, 18, 1, 12, 75, 3, 7 },
                    { 23, 17, 2, 20, 60, 3, 7 },
                    { 24, 19, 3, 30, 45, 3, 7 },
                    { 25, 3, 1, 10, 75, 4, 8 },
                    { 26, 4, 2, 15, 45, 3, 8 },
                    { 27, 13, 3, 12, 60, 3, 8 },
                    { 28, 14, 4, 15, 45, 3, 8 },
                    { 29, 2, 1, 8, 120, 4, 9 },
                    { 30, 3, 2, 10, 90, 3, 9 },
                    { 31, 4, 3, 15, 45, 3, 9 },
                    { 32, 1, 4, 15, 60, 3, 9 },
                    { 33, 5, 1, 6, 120, 4, 10 },
                    { 34, 6, 2, 10, 90, 4, 10 },
                    { 35, 8, 3, 12, 60, 3, 10 },
                    { 36, 11, 4, 15, 45, 3, 10 },
                    { 37, 15, 1, 8, 120, 4, 11 },
                    { 38, 2, 2, 10, 90, 3, 11 },
                    { 39, 6, 3, 10, 90, 3, 11 },
                    { 40, 9, 4, 10, 90, 3, 11 },
                    { 41, 19, 5, 45, 45, 3, 11 },
                    { 42, 15, 1, 5, 180, 5, 12 },
                    { 43, 16, 2, 8, 120, 4, 12 },
                    { 44, 17, 3, 20, 90, 3, 12 },
                    { 45, 18, 4, 12, 90, 3, 12 },
                    { 46, 20, 5, 12, 60, 3, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "WorkoutExercises",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "ExerciseId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "WorkoutId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "PlanId",
                keyValue: "plan-core-conditioning");

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "PlanId",
                keyValue: "plan-fat-burn");

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "PlanId",
                keyValue: "plan-foundation");

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "PlanId",
                keyValue: "plan-lower-power");

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "PlanId",
                keyValue: "plan-upper-strength");
        }
    }
}
