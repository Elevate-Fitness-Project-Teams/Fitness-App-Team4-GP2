using AuthService.BuildingBlocks.Extention;
using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Persistence.Repositories;
using AuthService.Shared.Configurations;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthServices();
            builder.Services.AddValidationConfiguration();

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
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
                app.UseSwaggerUI();

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
