namespace WorkoutService.BuildingBlock.HandlersResponse
{
    public class HandlerResponseFactory
    {
        public static HandlerResponse<T> Success<T>(T data) => new()
        {   
            IsSuccess = true,
            Data = data,
        };

        public static HandlerResponse<T> Failure<T>(HandlerErrorCodesEnum errorCode) => new()
        {
            IsSuccess = false,
            ErrorCode = errorCode,
        };
    }
}
