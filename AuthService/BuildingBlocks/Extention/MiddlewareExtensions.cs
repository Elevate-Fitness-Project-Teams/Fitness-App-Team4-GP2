using AuthService.BuildingBlocks.Middleware;

namespace AuthService.BuildingBlocks.Extention
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder
            UseGlobalExceptionMiddleware(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
