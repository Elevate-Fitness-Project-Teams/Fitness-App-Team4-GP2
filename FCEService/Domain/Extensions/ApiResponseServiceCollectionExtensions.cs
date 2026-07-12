using BuildingBlocks.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FCEService.Domain.Extensions
{
   
        public static class ApiResponseServiceCollectionExtensions
        {
          
            public static IServiceCollection AddApiResponseConventions(this IServiceCollection services)
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
}

