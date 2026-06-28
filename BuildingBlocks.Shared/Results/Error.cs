namespace BuildingBlocks.Shared.Results
{
    /// <summary>
    /// A typed failure carrying a stable machine-readable <see cref="Code"/> (e.g. "AUTH_TOKEN_INVALID"),
    /// a human-readable <see cref="Description"/>, and an <see cref="ErrorType"/> used to derive an HTTP status.
    /// </summary>
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public static Error Failure(string code = "General.Failure", string description = "A failure has occurred.")
            => new(code, description, ErrorType.Failure);

        public static Error Validation(string code = "General.Validation", string description = "A validation error has occurred.")
            => new(code, description, ErrorType.Validation);

        public static Error NotFound(string code = "General.NotFound", string description = "The requested resource was not found.")
            => new(code, description, ErrorType.NotFound);

        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Unauthorized.")
            => new(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden.")
            => new(code, description, ErrorType.Forbidden);

        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Invalid credentials.")
            => new(code, description, ErrorType.InvalidCredentials);

        public static Error Conflict(string code = "General.Conflict", string description = "A conflict has occurred.")
            => new(code, description, ErrorType.Conflict);

        public static Error Locked(string code = "General.Locked", string description = "The resource is locked.")
            => new(code, description, ErrorType.Locked);

        /// <summary>Maps this error's <see cref="ErrorType"/> to the HTTP status code used in the response envelope.</summary>
        public int ToStatusCode() => Type switch
        {
            ErrorType.Validation => 400,
            ErrorType.Unauthorized => 401,
            ErrorType.InvalidCredentials => 401,
            ErrorType.Forbidden => 403,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Locked => 423,
            ErrorType.Failure => 500,
            _ => 500
        };
    }
}
