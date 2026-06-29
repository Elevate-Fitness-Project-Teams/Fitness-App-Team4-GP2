using WorkoutService.Infrastructure.Persistence.Repositries;

namespace WorkoutService
{
    public static class DependacyInjection
    {
        public static IServiceCollection AddWorkoutServices(this IServiceCollection services)
        {
            // Register feature handlers, infrastructure services, validators, etc.
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenaricRepository<>));

            return services;
        }
    }
}
