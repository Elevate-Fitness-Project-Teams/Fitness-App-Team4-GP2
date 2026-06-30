using BuildingBlocks.Shared.Results;

namespace BuildingBlocks.Shared.Responses
{
    /// <summary>
    /// Maps an application-layer <see cref="Result{T}"/> onto the unified
    /// <see cref="ApiResponse{T}"/> wire envelope.
    /// </summary>
    public static class ResultExtensions
    {
        /// <param name="successMessage">Message to surface when the result is successful.</param>
        /// <param name="successStatusCode">HTTP status to use on success (e.g. 200, 201).</param>
        public static ApiResponse<T> ToApiResponse<T>(
            this Result<T> result,
            string successMessage,
            int successStatusCode)
        {
            if (result.IsSuccess)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = true,
                    Message = successMessage,
                    Data = result.Value,
                    Errors = new Dictionary<string, List<string>>(),
                    StatusCode = successStatusCode,
                    Timestamp = DateTime.UtcNow
                };
            }

            // Status code is derived from the first error's type; all error codes are surfaced.
            var primary = result.Errors[0];

            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = primary.Description,
                Data = default,
                Errors = result.Errors.GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList()),
                StatusCode = primary.ToStatusCode(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
