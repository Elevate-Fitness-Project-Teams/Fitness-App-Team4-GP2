using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Shared.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
