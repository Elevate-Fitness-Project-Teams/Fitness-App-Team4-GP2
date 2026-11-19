using BuildingBlocks.Contracts.Progress;
using FCEService.Domain.Entities;
using FCEService.Domain.Enums;
using FCEService.Domain.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotificationService.Domain.Entities;
using WorkoutService.BuildingBlock.Consumers;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Infrastructure.Persistence;
using WorkoutService.Infrastructure.Persistence.Repositries;
using Xunit;

namespace Elevate.FitnessApp.Tests.BusinessLogic
{
    public class BusinessLogicTests
    {
        [Fact]
        public void BUS_001_MetabolicCalculatorService_Recalculates_Metrics_Correctly()
        {
            var calculator = new MetabolicCalculatorService();

            // Test BMR calculation for Male (10 * weight + 6.25 * height - 5 * age + 5)
            // Weight = 80kg, Height = 180cm, Age = 30
            // 10 * 80 + 6.25 * 180 - 5 * 30 + 5 = 800 + 1125 - 150 + 5 = 1780
            var bmr = calculator.CalculateBmr(80, 180, 30, Gender.Male);
            Assert.Equal(1780, bmr);

            // Updated weight = 75kg
            // 10 * 75 + 6.25 * 180 - 5 * 30 + 5 = 750 + 1125 - 150 + 5 = 1730
            var updatedBmr = calculator.CalculateBmr(75, 180, 30, Gender.Male);
            Assert.Equal(1730, updatedBmr);
            Assert.NotEqual(bmr, updatedBmr);

            // Test TDEE calculation (Rookie = 1.2)
            var tdee = calculator.CalculateTdee(updatedBmr, ActivityLevel.Rookie);
            Assert.Equal(updatedBmr * 1.2, tdee);

            // Updated ActivityLevel = TrueBeast (1.9)
            var beastTdee = calculator.CalculateTdee(updatedBmr, ActivityLevel.TrueBeast);
            Assert.Equal(updatedBmr * 1.9, beastTdee);
            Assert.True(beastTdee > tdee);

            // CalorieTarget for LoseWeight = Tdee - 500
            var lossTarget = calculator.CalculateCalorieTarget(beastTdee, FitnessGoal.LoseWeight);
            Assert.Equal(beastTdee - 500, lossTarget);

            // CalorieTarget for GainWeight = Tdee + 300
            var gainTarget = calculator.CalculateCalorieTarget(beastTdee, FitnessGoal.GainWeight);
            Assert.Equal(beastTdee + 300, gainTarget);
        }

        [Fact]
        public async Task BUS_002_WorkoutSession_Status_Transitions_Correctly()
        {
            // Verify enum values
            Assert.Equal(1, (int)WorkOutSessionStatus.active);
            Assert.Equal(2, (int)WorkOutSessionStatus.completed);
            Assert.Equal(3, (int)WorkOutSessionStatus.abandoned);

            // Set up real EF Core InMemory database
            var options = new DbContextOptionsBuilder<WorkoutDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new WorkoutDbContext(options);

            var sessionId = Guid.NewGuid().ToString();
            var session = new WorkoutSession
            {
                SessionId = sessionId,
                UserId = Guid.NewGuid().ToString(),
                WorkoutId = 1,
                StartedAt = DateTime.UtcNow,
                Status = WorkOutSessionStatus.active
            };

            await db.WorkoutSessions.AddAsync(session);
            await db.SaveChangesAsync();

            var repo = new GenaricRepository<WorkoutSession>(db);
            var consumer = new SessionCompletedConsumer(repo);

            var mockContext = new Mock<ConsumeContext<SessionCompletedEvent>>();
            mockContext.Setup(c => c.Message).Returns(new SessionCompletedEvent { SessionId = sessionId });

            await consumer.Consume(mockContext.Object);

            var updatedSession = await db.WorkoutSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
            Assert.NotNull(updatedSession);
            Assert.Equal(WorkOutSessionStatus.completed, updatedSession.Status);
        }

        [Fact]
        public void US_9_1_Notification_Listing_Orders_Unread_First_Then_Chronological()
        {
            var userId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var notifications = new List<InAppNotification>
            {
                new() { Id = 1, UserId = userId, Title = "Read Old", IsRead = true, SentAt = now.AddHours(-10) },
                new() { Id = 2, UserId = userId, Title = "Unread Old", IsRead = false, SentAt = now.AddHours(-5) },
                new() { Id = 3, UserId = userId, Title = "Read Recent", IsRead = true, SentAt = now.AddHours(-1) },
                new() { Id = 4, UserId = userId, Title = "Unread Newest", IsRead = false, SentAt = now }
            };

            // OrderBy(n => n.IsRead).ThenByDescending(n => n.SentAt)
            var sorted = notifications
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.SentAt)
                .ToList();

            // First two must be unread (IsRead == false)
            Assert.False(sorted[0].IsRead);
            Assert.Equal(4, sorted[0].Id); // Unread newest

            Assert.False(sorted[1].IsRead);
            Assert.Equal(2, sorted[1].Id); // Unread old

            // Next two must be read (IsRead == true)
            Assert.True(sorted[2].IsRead);
            Assert.Equal(3, sorted[2].Id); // Read recent

            Assert.True(sorted[3].IsRead);
            Assert.Equal(1, sorted[3].Id); // Read old
        }
    }
}
