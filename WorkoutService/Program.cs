using BuildingBlocks.Shared.Persistence;
using BuildingBlocks.Shared.Middleware;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WorkoutService.BuildingBlock.Consumers;
using WorkoutService.Infrastructure.Persistence;

namespace WorkoutService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<WorkoutDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter the JWT access token (without the 'Bearer ' prefix).",
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                options.AddSecurityDefinition("Bearer", scheme);
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                });
            });


            builder.Services.AddWorkoutServices(); 
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


            builder.Services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   var jwt = builder.Configuration.GetSection("Jwt");
                   var key = jwt["Key"]
                       ?? throw new InvalidOperationException("Jwt:Key is not configured.");

                   options.TokenValidationParameters =
                   new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = jwt["Issuer"],
                       ValidAudience = jwt["Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(key)),
                       ClockSkew = TimeSpan.Zero
                   };
               });


            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<SessionCompletedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Create/upgrade this service's database and apply the HasData seed rows.
            // Runs before the pipeline is built so the app never serves traffic against a
            // missing schema; set Database:AutoMigrate=false to opt out.
            app.MigrateDatabase<WorkoutDbContext>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.EnablePersistAuthorization();
                    options.UseResponseInterceptor(
                        "(res) => {" +
                        "  try {" +
                        "    if (res.ok && res.url && res.url.toLowerCase().indexOf('/auth/login') !== -1) {" +
                        "      var body = res.obj || (res.text ? JSON.parse(res.text) : null);" +
                        "      var token = body && body.data && (body.data.token || body.data.accessToken);" +
                        "      if (token && window.ui) {" +
                        "        window.ui.preauthorizeApiKey('Bearer', token);" +
                        "        console.log('[Swagger] Bearer token captured from login.');" +
                        "      }" +
                        "    }" +
                        "  } catch (e) { console.error('[Swagger] token capture failed', e); }" +
                        "  return res;" +
                        "}");
                });

                // Proactively validate EF Core model structure at startup
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
                    var _ = db.Model; // Force compilation of model metadata
                }
            }

            

            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirectionOutsideContainer();

            app.UseRouting();
            app.UseCors("AngularPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
