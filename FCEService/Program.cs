using BuildingBlocks.Shared.Middleware;
using FCEService.Domain.Extensions;
using FCEService.Extensions;
using FCEService.Infrastructure.Persistence;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddMessaging(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FCEDbContext>();
                _ = db.Model;
            }

            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}