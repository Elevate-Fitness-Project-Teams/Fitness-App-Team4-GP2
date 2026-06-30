namespace BuildingBlocks.Shared.Responses
{
    /// <summary>
    /// The unified standard response envelope every microservice serializes at its boundary.
    /// Shape must stay identical across services:
    /// { isSuccess, message, data, errors, statusCode, timestamp }.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new();
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
