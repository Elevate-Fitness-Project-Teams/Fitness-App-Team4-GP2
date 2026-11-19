using ApiGateway.RateLimiting;
using StackExchange.Redis;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure centralized CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Configure Redis connection multiplexer safely (with fallback)
            var redisConn = builder.Configuration.GetConnectionString("Redis")
                            ?? builder.Configuration["ConnectionStrings:Redis"]
                            ?? "redis:6379";

            try
            {
                var redisOptions = ConfigurationOptions.Parse(redisConn);
                redisOptions.AbortOnConnectFail = false;
                redisOptions.ConnectTimeout = 2000;
                var redis = ConnectionMultiplexer.Connect(redisOptions);
                builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ApiGateway] Notice: Redis not reachable ({ex.Message}). Rate limiting will safely use the in-memory fallback.");
            }

            // Configure YARP reverse proxy from appsettings configuration
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            var app = builder.Build();

            app.UseCors();

            // Tier-based rate limiting (Anonymous: 50/hr, Free: 500/hr, Premium: 2000/hr)
            app.UseMiddleware<TierRateLimitingMiddleware>();

            app.MapReverseProxy();

            app.MapGet("/health", () => Results.Ok(new { status = "Healthy", service = "ApiGateway", timestamp = DateTime.UtcNow }));

            app.Run();
        }
    }
}

