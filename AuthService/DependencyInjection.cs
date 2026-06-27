using AuthService.Infrastructure.Services;
using AuthService.Infrastructure.Services.Interfaces;

namespace AuthService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            // Register feature handlers, infrastructure services, validators, etc.
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
