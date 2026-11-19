using System.Net;
using BuildingBlocks.Contracts.Progress;
using BuildingBlocks.Contracts.Subscription;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.Persistence;
using NutritionService.Infrastructure.Persistence.Integrations;
using ProfileService.Domain.Entities;
using ProfileService.Infrastructure.Persistence;
using ProfileService.Infrastructure.Persistence.Consumers;
using Xunit;

namespace Elevate.FitnessApp.Tests.Integration
{
    public class EventIntegrationTests
    {
        [Fact]
        public async Task MISS_004_SubscriptionUpgradedConsumer_Updates_Profile_IsPremiumCached()
        {
            var options = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new ProfileDbContext(options);

            var userId = Guid.NewGuid();
            var profile = new UserProfile
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                IsPremiumCached = false,
                MemberSince = DateTime.UtcNow
            };

            await db.UserProfiles.AddAsync(profile);
            await db.SaveChangesAsync();

            var consumer = new SubscriptionUpgradedConsumer(db, NullLogger<SubscriptionUpgradedConsumer>.Instance);

            var message = new SubscriptionUpgradedEvent
            {
                UserId = userId,
                Plan = "PremiumMonthly",
                IsPremium = true,
                UpgradedAt = DateTime.UtcNow
            };

            var mockContext = new Mock<ConsumeContext<SubscriptionUpgradedEvent>>();
            mockContext.Setup(c => c.Message).Returns(message);

            await consumer.Consume(mockContext.Object);

            var updatedProfile = await db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            Assert.NotNull(updatedProfile);
            Assert.True(updatedProfile.IsPremiumCached, "IsPremiumCached must be updated to true.");
        }

        [Fact]
        public async Task MISS_005_AchievementEarnedConsumer_Creates_Notification_And_Prevents_Duplicates()
        {
            var options = new DbContextOptionsBuilder<NotificationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new NotificationDbContext(options);

            var userId = Guid.NewGuid();
            var consumer = new AchievementEarnedConsumer(db, NullLogger<AchievementEarnedConsumer>.Instance);

            var achievementEvent = new AchievementEarnedEvent
            {
                UserId = userId.ToString(),
                AchievementId = 10,
                AchievementName = "Iron Warrior",
                EarnedAt = DateTime.UtcNow
            };

            var mockContext = new Mock<ConsumeContext<AchievementEarnedEvent>>();
            mockContext.Setup(c => c.Message).Returns(achievementEvent);

            // First delivery
            await consumer.Consume(mockContext.Object);

            var notifications = await db.InAppNotifications.Where(n => n.UserId == userId).ToListAsync();
            Assert.Single(notifications);
            Assert.Equal(NotificationType.AchievementAlert, notifications[0].Type);
            Assert.Contains("Iron Warrior", notifications[0].Message);
            Assert.False(notifications[0].IsRead);

            // Duplicate delivery (idempotency check)
            await consumer.Consume(mockContext.Object);

            var notificationsAfterDuplicate = await db.InAppNotifications.Where(n => n.UserId == userId).ToListAsync();
            Assert.Single(notificationsAfterDuplicate); // Must not create duplicate notification
        }

        [Fact]
        public async Task ERR_001_FceClient_Distinguishes_Missing_Metrics_From_Downstream_Failure()
        {
            // 1. Simulate 404 Not Found (User has not calculated metrics yet -> maps to 400 Validation)
            var mockHandler404 = new MockHttpMessageHandler(HttpStatusCode.NotFound, "");
            var httpClient404 = new HttpClient(mockHandler404) { BaseAddress = new Uri("http://localhost:5000") };
            var fceClient404 = new FceClient(httpClient404, NullLogger<FceClient>.Instance);

            var result404 = await fceClient404.GetMetricsAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.True(result404.IsFailure);
            Assert.Equal("FCE_METRICS_NOT_CALCULATED", result404.Errors[0].Code);
            Assert.Equal(400, result404.Errors[0].ToStatusCode());

            // 2. Simulate 500 Internal Server Error (Downstream failure -> maps to 503 Service Unavailable)
            var mockHandler500 = new MockHttpMessageHandler(HttpStatusCode.InternalServerError, "");
            var httpClient500 = new HttpClient(mockHandler500) { BaseAddress = new Uri("http://localhost:5000") };
            var fceClient500 = new FceClient(httpClient500, NullLogger<FceClient>.Instance);

            var result500 = await fceClient500.GetMetricsAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.True(result500.IsFailure);
            Assert.Equal("SRV_SERVICE_UNAVAILABLE", result500.Errors[0].Code);
            Assert.Equal(503, result500.Errors[0].ToStatusCode());

            // 3. Simulate Connection Failure (HttpRequestException -> maps to 503 Service Unavailable)
            var mockHandlerFailure = new FailingHttpMessageHandler();
            var httpClientFailure = new HttpClient(mockHandlerFailure) { BaseAddress = new Uri("http://localhost:5000") };
            var fceClientFailure = new FceClient(httpClientFailure, NullLogger<FceClient>.Instance);

            var resultFailure = await fceClientFailure.GetMetricsAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.True(resultFailure.IsFailure);
            Assert.Equal("SRV_SERVICE_UNAVAILABLE", resultFailure.Errors[0].Code);
            Assert.Equal(503, resultFailure.Errors[0].ToStatusCode());
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _content;

        public MockHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_content)
            };
            return Task.FromResult(response);
        }
    }

    public class FailingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new HttpRequestException("Connection refused by downstream service.");
        }
    }
}
