using BuildingBlocks.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

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
                    Errors = errors,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Timestamp = DateTime.UtcNow
                };

                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }
}