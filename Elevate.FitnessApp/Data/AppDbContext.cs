using Microsoft.EntityFrameworkCore;
using Elevate.FitnessApp.Features.Auth.Entities;
using Elevate.FitnessApp.Features.Profiles.Entities;
using Elevate.FitnessApp.Features.FitnessCalculation.Entities;
using Elevate.FitnessApp.Features.Workouts.Entities;
using Elevate.FitnessApp.Features.Nutrition.Entities;
using Elevate.FitnessApp.Features.SmartCoach.Entities;
using Elevate.FitnessApp.Features.Progress.Entities;
using Elevate.FitnessApp.Features.Subscriptions.Entities;
using Elevate.FitnessApp.Features.Notifications.Entities;

namespace Elevate.FitnessApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Auth Slice DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<OtpCode> OtpCodes { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<LoginAttempt> LoginAttempts { get; set; } = null!;

        // Profiles Slice DbSets
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;
        public DbSet<NotificationSetting> NotificationSettings { get; set; } = null!;
        public DbSet<PrivacySetting> PrivacySettings { get; set; } = null!;

        // FitnessCalculation Slice DbSets
        public DbSet<UserFitnessStat> UserFitnessStats { get; set; } = null!;
        public DbSet<CalculatedMetric> CalculatedMetrics { get; set; } = null!;
        public DbSet<FitnessPlanConfig> FitnessPlanConfigs { get; set; } = null!;
        public DbSet<UserAssignedPlan> UserAssignedPlans { get; set; } = null!;
        public DbSet<UserPlanHistory> UserPlanHistories { get; set; } = null!;

        // Workouts Slice DbSets
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; } = null!;
        public DbSet<Workout> Workouts { get; set; } = null!;
        public DbSet<Exercise> Exercises { get; set; } = null!;
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = null!;
        public DbSet<WorkoutSession> WorkoutSessions { get; set; } = null!;

        // Nutrition Slice DbSets
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<MealPlan> MealPlans { get; set; } = null!;
        public DbSet<MealPlanItem> MealPlanItems { get; set; } = null!;

        // SmartCoach Slice DbSets
        public DbSet<ChatSession> ChatSessions { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<RecommendationCache> RecommendationCaches { get; set; } = null!;

        // Progress Slice DbSets
        public DbSet<WorkoutLog> WorkoutLogs { get; set; } = null!;
        public DbSet<WorkoutLogExercise> WorkoutLogExercises { get; set; } = null!;
        public DbSet<WeightHistory> WeightHistories { get; set; } = null!;
        public DbSet<BodyMeasurement> BodyMeasurements { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public DbSet<UserAchievement> UserAchievements { get; set; } = null!;
        public DbSet<Streak> Streaks { get; set; } = null!;
        public DbSet<UserStatistic> UserStatistics { get; set; } = null!;

        // Subscriptions Slice DbSets
        public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
        public DbSet<BillingLog> BillingLogs { get; set; } = null!;

        // Notifications Slice DbSets
        public DbSet<InAppNotification> InAppNotifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. Auth Slice Configurations
            // ==========================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(512).IsRequired();
                entity.Property(e => e.IsLockedOut).IsRequired();
                entity.Property(e => e.LockedUntil).IsRequired(false);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<OtpCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email);
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Code).HasMaxLength(6).IsRequired();
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.Property(e => e.IsUsed).IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.Token).HasMaxLength(512).IsRequired();
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.RevokedAt).IsRequired(false);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoginAttempt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.AttemptedAt).IsRequired();
                entity.Property(e => e.IsSuccess).IsRequired();
                entity.Property(e => e.IpAddress).HasMaxLength(45).IsRequired();
            });

            // ==========================================
            // 2. Profiles Slice Configurations
            // ==========================================
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500).IsRequired(false);
                entity.Property(e => e.IsPremiumCached).IsRequired();
                entity.Property(e => e.MemberSince).IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Restrict to avoid multiple cascade paths in SQL Server
            });

            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Language).HasMaxLength(10).HasDefaultValue("en").IsRequired();
                entity.Property(e => e.Theme).HasMaxLength(15).HasDefaultValue("light").IsRequired();
                entity.Property(e => e.WeightUnit).HasMaxLength(5).HasDefaultValue("kg").IsRequired();
                entity.Property(e => e.HeightUnit).HasMaxLength(5).HasDefaultValue("cm").IsRequired();
                entity.Property(e => e.DistanceUnit).HasMaxLength(5).HasDefaultValue("km").IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<UserPreference>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationSetting>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.WorkoutReminders).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.MealReminders).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.AchievementAlerts).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.WeeklyReports).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.EmailNotifications).HasDefaultValue(true).IsRequired();
                entity.Property(e => e.PushNotifications).HasDefaultValue(true).IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<NotificationSetting>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PrivacySetting>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.ProfileVisibility).HasMaxLength(20).HasDefaultValue("private").IsRequired();
                entity.Property(e => e.ShowProgressToFriends).HasDefaultValue(false).IsRequired();
                entity.Property(e => e.AllowDataSharing).HasDefaultValue(false).IsRequired();

                entity.HasOne(e => e.UserProfile)
                    .WithOne()
                    .HasForeignKey<PrivacySetting>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 3. FitnessCalculation Slice Configurations
            // ==========================================
            modelBuilder.Entity<UserFitnessStat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Gender).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ActivityLevel).HasMaxLength(30).IsRequired();
                entity.Property(e => e.RecordedAt).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CalculatedMetric>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CalculatedAt).IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<CalculatedMetric>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FitnessPlanConfig>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasMaxLength(50);
                entity.Property(e => e.PlanName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.EstimatedDuration).HasMaxLength(100).IsRequired();
                entity.Property(e => e.WorkoutsPerWeek).IsRequired();
                entity.Property(e => e.ProgramType).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<UserAssignedPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.AssignedAt).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FitnessPlanConfig)
                    .WithMany()
                    .HasForeignKey(e => e.PlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserPlanHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.AssignedAt).IsRequired();
                entity.Property(e => e.EndedAt).IsRequired(false);
                entity.Property(e => e.ReasonForChange).HasMaxLength(255).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 4. Workouts Slice Configurations
            // ==========================================
            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Goal).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(e => e.WorkoutId);
                entity.HasIndex(e => e.Category);
                entity.Property(e => e.PlanId).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany()
                    .HasForeignKey(e => e.PlanId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.ExerciseId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.TargetMuscles).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Equipment).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Difficulty).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.VideoUrl).HasMaxLength(500).IsRequired(false);
            });

            modelBuilder.Entity<WorkoutExercise>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Workout)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Exercise)
                    .WithMany()
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkoutSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Workout)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 5. Nutrition Slice Configurations
            // ==========================================
            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => e.MealId);
                entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
                entity.Property(e => e.IngredientsJson).IsRequired(); // NVARCHAR(MAX) by default in EF Core for string
                entity.Property(e => e.InstructionsJson).IsRequired();
                entity.Property(e => e.VariationsJson).IsRequired(false);
                entity.Property(e => e.AllergensJson).HasMaxLength(500).IsRequired();
                entity.Property(e => e.TagsJson).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<MealPlan>(entity =>
            {
                entity.HasKey(e => e.MealPlanId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            });

            modelBuilder.Entity<MealPlanItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DayOfWeek).HasMaxLength(15).IsRequired();
                entity.Property(e => e.MealTime).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.MealPlan)
                    .WithMany()
                    .HasForeignKey(e => e.MealPlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Meal)
                    .WithMany()
                    .HasForeignKey(e => e.MealId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 6. SmartCoach Slice Configurations
            // ==========================================
            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.HasKey(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Title).HasMaxLength(150).HasDefaultValue("New Conversation").IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.Property(e => e.SessionId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Sender).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.ChatSession)
                    .WithMany()
                    .HasForeignKey(e => e.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecommendationCache>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserContextJson).IsRequired();
                entity.Property(e => e.HomeFeedDataJson).IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<RecommendationCache>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 7. Progress Slice Configurations
            // ==========================================
            modelBuilder.Entity<WorkoutLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.SessionId);
                entity.Property(e => e.SessionId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(1000).IsRequired(false);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkoutLogExercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WeightUsed).HasDefaultValue(0);

                entity.HasOne(e => e.WorkoutLog)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutLogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WeightHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Notes).HasMaxLength(500).IsRequired(false);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BodyMeasurement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IconUrl).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Achievement)
                    .WithMany()
                    .HasForeignKey(e => e.AchievementId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Streak>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.CurrentStreak).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.LongestStreak).HasDefaultValue(0).IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Streak>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserStatistic>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.TotalWorkouts).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.TotalCaloriesBurned).HasDefaultValue(0).IsRequired();
                entity.Property(e => e.TotalWeightLost).HasDefaultValue(0).IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<UserStatistic>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ==========================================
            // 8. Subscriptions Slice Configurations
            // ==========================================
            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.Property(e => e.Tier).HasMaxLength(20).HasDefaultValue("Free").IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active").IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<UserSubscription>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BillingLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.ReferenceId).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("EGP").IsRequired();
                entity.Property(e => e.PaymentStatus).HasMaxLength(20).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // 9. Notifications Slice Configurations
            // ==========================================
            modelBuilder.Entity<InAppNotification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.Title).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Message).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Type).HasMaxLength(30).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
