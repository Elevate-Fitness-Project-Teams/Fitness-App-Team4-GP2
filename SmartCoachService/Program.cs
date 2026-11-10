using BuildingBlocks.Shared.Persistence;
using BuildingBlocks.Shared.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartCoachService.Domain.Contracts;
using SmartCoachService.Infrastructure.Persistence;
using SmartCoachService.Infrastructure.Persistence.AI;
using SmartCoachService.Infrastructure.Persistence.Caching;
using SmartCoachService.Infrastructure.Persistence.Integrations;
using SmartCoachService.Infrastructure.Persistence.Integrations.Services;
using SmartCoachService.Infrastructure.Persistence.Repositories;
using StackExchange.Redis;
using System.Text;

namespace SmartCoachService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<SmartCoachDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = builder.Configuration.GetConnectionString("Redis")
                                      ?? "localhost:6379";
                var redisOptions = ConfigurationOptions.Parse(redisConnection);
                redisOptions.AbortOnConnectFail = false;

                options.ConfigurationOptions = redisOptions;
                options.InstanceName = "SmartCoach:";
            });


            builder.Services.AddScoped<ICacheService, RedisCacheService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });


            builder.Services.AddTransient<AuthHeaderForwardingHandler>();

            // FitnessController and ProgressController are both [Authorize], so these two
            // clients must forward the caller's bearer token like the other three do -
            // without it they get a 401 and the aggregator sees null.
            builder.Services.AddHttpClient<FceClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["Services:Fce"]!);
            }).AddHttpMessageHandler<AuthHeaderForwardingHandler>();

            builder.Services.AddHttpClient<ProgressClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["Services:Progress"]!);

            }).AddHttpMessageHandler<AuthHeaderForwardingHandler>();

            builder.Services.AddHttpClient<ProfileClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["Services:Profile"]!);
            }).AddHttpMessageHandler<AuthHeaderForwardingHandler>();

            builder.Services.AddHttpClient<WorkoutClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["Services:Workout"]!);
            }).AddHttpMessageHandler<AuthHeaderForwardingHandler>();

            builder.Services.AddHttpClient<NutritionClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["Services:Nutrition"]!);
            }).AddHttpMessageHandler<AuthHeaderForwardingHandler>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            builder.Services.AddScoped<IAiService, GeminiService>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();


            var app = builder.Build();

            // Create/upgrade this service's database and apply the HasData seed rows.
            // Runs before the pipeline is built so the app never serves traffic against a
            // missing schema; set Database:AutoMigrate=false to opt out.
            app.MigrateDatabase<SmartCoachDbContext>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Proactively validate EF Core model structure at startup
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SmartCoachDbContext>();
                    var _ = db.Model; // Force compilation of model metadata
                }
            }

            // Translate unhandled exceptions into the unified ApiResponse envelope
            // (without this, failures leak raw stack traces to the client).
            app.UseGlobalExceptionMiddleware();

            app.UseHttpsRedirectionOutsideContainer();

            // Authentication must run before authorization so [Authorize] endpoints
            // can resolve the JWT-derived user (ICurrentUser reads HttpContext.User).
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
