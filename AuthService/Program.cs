using BuildingBlocks.Shared.Middleware;
using AuthService.BuildingBlocks.Interfaces;
using AuthService.BuildingBlocks.Interfaces.Events;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Repositories;
using AuthService.Infrastructure.Services;
using AuthService.Infrastructure.Services.Interfaces;
using AuthService.Shared.Configurations;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IApplicationDbContext>(
                sp => sp.GetRequiredService<AuthDbContext>());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
            builder.Services.AddScoped<IOtpRepository, OtpRepository>();

            // Email: log to console in Development (no SMTP needed), send via SMTP otherwise.
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
            if (builder.Environment.IsDevelopment())
                builder.Services.AddScoped<IEmailService, DevEmailService>();
            else
                builder.Services.AddScoped<IEmailService, SmtpEmailService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IEventPublisher, RabbitMqPublisher>();
            builder.Services.AddAuthServices();
            builder.Services.AddValidationConfiguration();

            builder.Services
                .AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });
                });
            });

            builder.Services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan =
                        TimeSpan.FromMinutes(15);

                    options.Lockout.AllowedForNewUsers = true;

                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

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

            var app = builder.Build(); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // Keep the captured Bearer token across page reloads / app restarts.
                    options.EnablePersistAuthorization();

                    // Auto-capture the access token from the /login response and apply it
                    // to the "Bearer" security scheme, so secured endpoints are authorized
                    // automatically without copy/paste into the Authorize dialog.
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
                    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                    var _ = db.Model; // Force compilation of model metadata
                }
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
