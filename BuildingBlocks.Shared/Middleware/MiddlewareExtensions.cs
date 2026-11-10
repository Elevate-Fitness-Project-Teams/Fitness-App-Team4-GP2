using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Shared.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();

        /// <summary>
        /// Applies HTTPS redirection only when the app is NOT running inside a container.
        ///
        /// Inside Docker the services call each other over plain HTTP on port 8080 across
        /// the compose network; TLS is terminated at the published host ports (or an
        /// upstream gateway), not between containers. Left on, UseHttpsRedirection()
        /// answers an internal http://service:8080 call with a 307 to that service's HTTPS
        /// port, which is not published on the internal network - so cross-service calls
        /// (e.g. the SmartCoach home feed aggregating Nutrition/Progress/FCE) fail with
        /// "connection refused". The ASP.NET base image sets DOTNET_RUNNING_IN_CONTAINER,
        /// which we key off here; running from the CLI/VS on the host still redirects.
        /// </summary>
        public static IApplicationBuilder UseHttpsRedirectionOutsideContainer(this IApplicationBuilder app)
        {
            var inContainer = string.Equals(
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
                "true",
                StringComparison.OrdinalIgnoreCase);

            if (!inContainer)
                app.UseHttpsRedirection();

            return app;
        }
    }
}
