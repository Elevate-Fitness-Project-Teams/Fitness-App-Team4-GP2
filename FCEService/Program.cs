using BuildingBlocks.Shared.Persistence;
using BuildingBlocks.Shared.Middleware;
using FCEService.Domain.Extensions;
using FCEService.Extensions;
using FCEService.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;

namespace FCEService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApiResponseConventions();
            builder.Services.AddEndpointsApiExplorer();
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

            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddMessaging(builder.Configuration);

            var app = builder.Build();

            // Create/upgrade this service's database and apply the HasData seed rows.
            // Runs before the pipeline is built so the app never serves traffic against a
            // missing schema; set Database:AutoMigrate=false to opt out.
            app.MigrateDatabase<FCEDbContext>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FCEDbContext>();
                _ = db.Model;
            }

            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirectionOutsideContainer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}