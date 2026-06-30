namespace BuildingBlocks.Shared.Responses
{
    /// <summary>
    /// Builds <see cref="ApiResponse{T}"/> envelopes directly. Prefer mapping a
    /// <c>Result&lt;T&gt;</c> via <see cref="ResultExtensions.ToApiResponse{T}"/>; use this for
    /// places that have no Result (e.g. middleware, model-validation factory).
    /// </summary>
    public static class ResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message, int statusCode) => new()
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };

        public static ApiResponse<object> Failure(string message, int statusCode, params string[] errors) => new()
        {
            IsSuccess = false,
            Message = message,
            Errors = errors.GroupBy(e => e)
                .ToDictionary(g => g.Key, g => g.ToList()),
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }
}
