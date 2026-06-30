using BuildingBlocks.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AuthService.Shared.Configurations;

public static class ValidationConfiguration
{
    public static IServiceCollection AddValidationConfiguration(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
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

        return services;
    }
}