namespace AuthService.Shared.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = [];
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
            = DateTime.UtcNow;
    }
}
