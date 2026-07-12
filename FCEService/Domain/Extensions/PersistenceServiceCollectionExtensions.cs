using FCEService.Domain.Interfaces;
using FCEService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Extensions
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FCEDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IFceUnitOfWork,FceUnitofWork>();

            return services;
        }
    }
}