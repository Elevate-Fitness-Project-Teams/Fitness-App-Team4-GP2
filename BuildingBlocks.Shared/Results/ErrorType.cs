namespace BuildingBlocks.Shared.Results
{
    /// <summary>
    /// Classifies a domain/application failure so it can be mapped to an HTTP status code.
    /// </summary>
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Unauthorized = 3,
        Forbidden = 4,
        InvalidCredentials = 5,
        Conflict = 6,
        Locked = 7,
        TooManyRequestsException =8
    }
}
