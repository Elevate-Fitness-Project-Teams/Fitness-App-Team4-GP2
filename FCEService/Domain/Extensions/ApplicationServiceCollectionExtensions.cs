using System.Reflection;
using BuildingBlocks.Shared.Behaviors;
using FCEService.Domain.Services;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;

namespace FCEService.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            var mapsterConfig = TypeAdapterConfig.GlobalSettings;
            mapsterConfig.Scan(assembly);
            services.AddSingleton(mapsterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddScoped<IMetabolicCalculatorService, MetabolicCalculatorService>();

            return services;
        }
    }
}