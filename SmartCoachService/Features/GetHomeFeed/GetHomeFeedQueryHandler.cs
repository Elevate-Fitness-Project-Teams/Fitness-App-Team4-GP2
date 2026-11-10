using BuildingBlocks.Shared.Results;
using MediatR;
using SmartCoachService.Domain.Contracts;
using SmartCoachService.Infrastructure.Persistence.Integrations;

namespace SmartCoachService.Features.GetHomeFeed
{
    /// <summary>
    /// Cache-first edge aggregator for the Home Feed (User Story 7.3), backed by Redis.
    ///  - Cache hit (key present and not expired): return the cached feed directly.
    ///  - Cache miss/expired: aggregate a fresh payload from Profile, FCE, Workout,
    ///    Nutrition and Progress, write it to the cache with a TTL, and return it.
    ///  - Any downstream aggregation call fails: return 503 SRV_SERVICE_UNAVAILABLE.
    ///
    /// A Redis outage degrades to a cache miss (see <see cref="ICacheService"/>), so the
    /// endpoint keeps serving freshly aggregated data instead of failing.
    /// </summary>
    public class GetHomeFeedQueryHandler : IRequestHandler<GetHomeFeedQuery, Result<HomeFeedResponse>>
    {
        private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);
        private const string CacheKeyPrefix = "smartcoach:homefeed:";

        private readonly ICacheService _cache;
        private readonly ProfileClient _profileClient;
        private readonly FceClient _fceClient;
        private readonly WorkoutClient _workoutClient;
        private readonly NutritionClient _nutritionClient;
        private readonly ProgressClient _progressClient;
        private readonly ILogger<GetHomeFeedQueryHandler> _logger;

        public GetHomeFeedQueryHandler(
            ICacheService cache,
            ProfileClient profileClient,
            FceClient fceClient,
            WorkoutClient workoutClient,
            NutritionClient nutritionClient,
            ProgressClient progressClient,
            ILogger<GetHomeFeedQueryHandler> logger)
        {
            _cache = cache;
            _profileClient = profileClient;
            _fceClient = fceClient;
            _workoutClient = workoutClient;
            _nutritionClient = nutritionClient;
            _progressClient = progressClient;
            _logger = logger;
        }

        public async Task<Result<HomeFeedResponse>> Handle(GetHomeFeedQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeyPrefix + request.UserId;

            // AC #1 - cache hit: return the cached feed directly.
            var cached = await _cache.GetAsync<HomeFeedResponse>(cacheKey, cancellationToken);
            if (cached is not null)
                return cached;

            // AC #2/#3 - cache miss/expired: aggregate a fresh payload in parallel.
            var profileTask = _profileClient.GetProfileAsync(cancellationToken);
            var metricsTask = _fceClient.GetMetricsAsync(request.UserId, cancellationToken);
            var workoutTask = _workoutClient.GetWorkOutsAsync(cancellationToken);
            var nutritionTask = _nutritionClient.GetMealRecommendationsAsync(request.UserId, cancellationToken);
            var progressTask = _progressClient.GetProgressStatsAsync(request.UserId, cancellationToken);

            // AC #3 - only a genuine outage (unreachable / timed out / 5xx) is a 503, and
            // the clients signal that by throwing. A 4xx means the service is healthy but
            // has no data for this user yet, which arrives here as null and renders as an
            // absent section rather than failing the whole feed.
            try
            {
                await Task.WhenAll(profileTask, metricsTask, workoutTask, nutritionTask, progressTask);
            }
            catch (DownstreamUnavailableException ex)
            {
                // DownstreamHttp has already logged the status code / exception.
                _logger.LogError(ex,
                    "Home feed aggregation for {UserId} failed; {Uri} is unavailable.",
                    request.UserId, ex.Uri);

                return Error.ServiceUnavailable(
                    "SRV_SERVICE_UNAVAILABLE",
                    "A downstream service required to build the home feed is unavailable.");
            }

            var profile = profileTask.Result;
            var metrics = metricsTask.Result;
            var workouts = workoutTask.Result;
            var nutrition = nutritionTask.Result;
            var progress = progressTask.Result;

            var feed = new HomeFeedResponse(
                Profile: profile is null ? null : new HomeFeedProfileSection(
                    DisplayName: $"{profile.FirstName} {profile.LastName}".Trim(),
                    ProfilePictureUrl: profile.ProfilePictureUrl,
                    Height: profile.FceProfile?.Height,
                    Weight: profile.FceProfile?.Weight),
                Metrics: metrics is null ? null : new HomeFeedMetricsSection(
                    metrics.Bmr,
                    metrics.Tdee,
                    metrics.CalorieTarget),
                SuggestedWorkouts: workouts?
                    .Select(w => new HomeFeedWorkoutItem(
                        w.WorkoutId,
                        w.Name,
                        w.Category,
                        w.DurationInMinutes,
                        w.Difficulty))
                    .ToList() ?? new List<HomeFeedWorkoutItem>(),
                Nutrition: nutrition is null ? null : new HomeFeedNutritionSection(
                    DailyGoalCalories: nutrition.UserDailyGoalCalories,
                    RecommendedMeals: nutrition.RecommendedMeals.Items
                        .Select(m => new HomeFeedMealItem(
                            m.MealId,
                            m.Name,
                            m.Type,
                            m.Calories,
                            m.Protein))
                        .ToList()),
                Progress: progress is null ? null : new HomeFeedProgressSection(
                    progress.TotalWorkouts,
                    progress.TotalCaloriesBurned,
                    progress.TotalWeightLost,
                    progress.CurrentStreak,
                    progress.LongestStreak),
                GeneratedAt: DateTime.UtcNow);

            // Upsert the aggregated feed into Redis with a sliding-window TTL.
            await _cache.SetAsync(cacheKey, feed, CacheTtl, cancellationToken);

            return feed;
        }
    }
}
