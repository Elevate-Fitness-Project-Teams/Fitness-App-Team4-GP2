using AuthService.BuildingBlocks.Helpers;

namespace AuthService.BuildingBlocks.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = 409;

                await context.Response.WriteAsJsonAsync(
                    ResponseFactory.Failure(
                        ex.Message,
                        409,
                        ex.Message));
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsJsonAsync(
                    ResponseFactory.Failure(
                        ex.Message,
                        400,
                        ex.Message));
            }
        }
    }
}
