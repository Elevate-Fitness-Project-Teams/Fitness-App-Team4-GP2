using BuildingBlocks.Shared.Middleware;
using BuildingBlocks.Shared.Responses;
using FCEService.Domain.Interfaces;
using FCEService.Domain.Services;
using FCEService.Infrastructure.Persistence;
using FCEService.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace FCEService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

           
        
           builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Validation failed.",
                        Data = null,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Timestamp = DateTime.UtcNow,
                        Errors = new Dictionary<string, List<string>> { { "ValidationErrors", errors } }

                    };

                    return new BadRequestObjectResult(response);
                };
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FCEDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(BuildingBlocks.Shared.Behaviors.ValidationBehavior<,>));
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddSingleton(config).AddScoped<IMapper, ServiceMapper>();



            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddScoped<IFceUnitOfWork,FceUnitofWork>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IFitnessPlanConfigRepository, FitnessPlanConfigRepository>();
            builder.Services.AddScoped<IMetabolicCalculatorService, MetabolicCalculatorService>();

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

             
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<FCEDbContext>();
                    var _ = db.Model; 
                }
            }

            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
