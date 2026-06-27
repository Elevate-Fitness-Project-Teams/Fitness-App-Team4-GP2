using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;
using ProgressService.Shared;
using ProgressService.Shared.Contracts;
using ProgressService.Shared.Events;
using System.Security.Cryptography.Xml;

namespace ProgressService.Features.WorkoutCompletion
{
    public class WorkoutCompletionHandler : IRequestHandler<WorkoutCompletionCommand, Result<WorkOutCompletionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<WorkoutCompletionCommand> _validator;
        private readonly IMessagePublisher _publisher;

        public WorkoutCompletionHandler(IUnitOfWork unitOfWork , IValidator<WorkoutCompletionCommand> validator , IMessagePublisher publisher)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _publisher = publisher;
        }

        public async Task<Result<WorkOutCompletionResponse>> Handle(WorkoutCompletionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            var userId = request.UserId;

            if (!validationResult.IsValid)
            {
                return Error.Validation("VAL_REQUIRED_FIELD", System.Text.Json.JsonSerializer.Serialize(
                                                                    validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var log = new WorkoutLog
                {
                    WorkoutId = request.WorkoutId,
                    UserId = userId,
                    SessionId = request.SessionId,
                    DurationInMinutes = request.DurationInMinutes,
                    CaloriesBurned = request.CaloriesBurned,
                    Rating = request.Rating,
                    Notes = request.Notes,
                    WorkoutLogExercises = request.ExercisesCompleted.Select(e => new WorkoutLogExercise
                    {
                        ExerciseId = e.ExerciseId,
                        SetsCompleted = e.SetsCompleted,
                        RepsCompleted = e.RepsCompleted,
                        WeightUsed = e.WeightUsed,
                    }).ToList()
                };

                _unitOfWork.GetRepository<WorkoutLog, int>().Add(log);

                var (streakUpdated, CurrentStreak) = await UpdateUserStreak(userId);
                var userStatistics = await UpdateUserStatistics(request);
                var metrics = new Dictionary<CriteriaType, int>
            {
                { CriteriaType.WORKOUT_COUNT, userStatistics.TotalWorkouts },
                { CriteriaType.CALORIES_BURNED, userStatistics.TotalCaloriesBurned },
                { CriteriaType.STREAK_DAYS, CurrentStreak }
            };

                var unlockedAchievements = await EvaluateAchievements(userId, metrics);



                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                foreach (var achievement in unlockedAchievements)
                {
                    await _publisher.PublishAsync(
                        new AchievementEarnedEvent
                        {
                            UserId = userId,
                            AchievementId = achievement.Id,
                            AchievementName = achievement.Name,
                            EarnedAt = DateTime.UtcNow
                        });
                }

                return new WorkOutCompletionResponse
                {
                    LogId = log.Id,
                    UpdatedStreak = streakUpdated,
                    CurrentStreak = CurrentStreak
                };

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        private async Task<(bool StreakUpdated, int CurrentStreak)> UpdateUserStreak(string userId)
        {
            var streak = await _unitOfWork.GetRepository<Streak, int>().GetOneAsync(x => x.UserId == userId);
            var streakUpdated = false;
            if (streak is not null)
            {
                 
                var today = DateTime.UtcNow.Date;
                if (streak.LastWorkoutDate == today.AddDays(-1))
                {
                    streak.CurrentStreak++;
                    streakUpdated = true;
                }

                else if (streak.LastWorkoutDate != today)
                {
                    streak.CurrentStreak = 1;
                    streakUpdated = true;
                }

                streak.LastWorkoutDate = today;

                if (streak.LongestStreak < streak.CurrentStreak)
                    streak.LongestStreak = streak.CurrentStreak;

                return(streakUpdated, streak.CurrentStreak);
            }
            streak = new Streak
            {
                UserId = userId,
                CurrentStreak = 1,
                LongestStreak = 1,
                LastWorkoutDate = DateTime.UtcNow.Date

            };

            _unitOfWork.GetRepository<Streak, int>().Add(streak);

            return (true,1);

        }

        private async Task<UserStatistic> UpdateUserStatistics(WorkoutCompletionCommand request)
        {
            var userStatistics = await _unitOfWork.GetRepository<UserStatistic, int>().GetOneAsync(x => x.UserId == request.UserId);

            if (userStatistics is not null)
            {
                userStatistics.TotalWorkouts++;
                userStatistics.TotalCaloriesBurned += request.CaloriesBurned;
                userStatistics.UpdatedAt = DateTime.UtcNow;
            }
            
            else
            {
                userStatistics = new UserStatistic
                {
                    UserId = request.UserId,
                    TotalWorkouts = 1,
                    TotalCaloriesBurned = request.CaloriesBurned
                };

                _unitOfWork.GetRepository<UserStatistic, int>().Add(userStatistics);
                
            }
            return userStatistics;
        }


        private async Task<List<Achievement>> EvaluateAchievements(string userId , Dictionary<CriteriaType , int> metrics)
        {
            var userAchievementsRepo = _unitOfWork.GetRepository<UserAchievement, int>();
            var achievements = await _unitOfWork.GetRepository<Achievement, int>().GetAll().ToListAsync();
            var userAchievementsIds = (await userAchievementsRepo
               .GetAll(X=> X.UserId == userId).Select(x=>x.AchievementId).ToListAsync()).ToHashSet();

            var unlockedAchievements = achievements
                .Where(a => !userAchievementsIds.Contains(a.Id))
                .Where(a =>
                    metrics.ContainsKey(a.CriteriaType) &&
                    metrics[a.CriteriaType] >= a.CriteriaValue)
                .ToList();

            foreach (var achievement in unlockedAchievements)
            {
                userAchievementsRepo.Add(new UserAchievement
                {
                    UserId = userId,
                    AchievementId = achievement.Id,
                    EarnedAt = DateTime.UtcNow
                });
            }
            return unlockedAchievements;



        }

    }
}


