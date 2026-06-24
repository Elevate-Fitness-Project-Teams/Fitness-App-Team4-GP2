using AuthService.Shared.Responses;

namespace AuthService.BuildingBlocks.Helpers
{
    public static class ResponseFactory
    {
        public static ApiResponse<T> Success<T>(
            T data,
            string message,
            int statusCode)
        {
            return new()
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse<object> Failure(
            string message,
            int statusCode,
            params string[] errors)
        {
            return new()
            {
                IsSuccess = false,
                Message = message,
                Errors = errors.ToList(),
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
