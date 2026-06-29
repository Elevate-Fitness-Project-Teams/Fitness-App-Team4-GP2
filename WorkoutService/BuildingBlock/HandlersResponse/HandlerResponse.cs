namespace WorkoutService.BuildingBlock.HandlersResponse
{
    public record HandlerResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public HandlerErrorCodesEnum ErrorCode { get; set; }

    }
}
