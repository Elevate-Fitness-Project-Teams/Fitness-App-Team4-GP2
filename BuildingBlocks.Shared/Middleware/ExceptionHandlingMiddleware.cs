using BuildingBlocks.Shared.Exceptions;
using BuildingBlocks.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Shared.Middleware
{
    /// <summary>
    /// Catches unhandled exceptions and writes the unified <see cref="ApiResponse{T}"/> envelope.
    /// Known <see cref="AppException"/>s map to their carried error/status; anything else is a
    /// 500 with no detail leaked to the client.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                await WriteAsync(context, ex.Error.ToStatusCode(), ex.Error.Description, ex.Error.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing {Path}", context.Request.Path);
                await WriteAsync(context, 500, "An unexpected error occurred.", "INTERNAL_SERVER_ERROR");
            }
        }

        private static async Task WriteAsync(HttpContext context, int statusCode, string message, string errorCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                ResponseFactory.Failure(message, statusCode, errorCode));
        }
    }
}
