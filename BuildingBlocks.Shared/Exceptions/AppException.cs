using BuildingBlocks.Shared.Results;

namespace BuildingBlocks.Shared.Exceptions
{
    /// <summary>
    /// An exception that carries a typed <see cref="Error"/>. Throw this for the rare
    /// cases where a failure must bubble up out-of-band; the global exception middleware
    /// turns it into the standard response envelope using the error's code and status.
    /// Prefer returning <c>Result&lt;T&gt;</c> from handlers over throwing.
    /// </summary>
    public class AppException : Exception
    {
        public Error Error { get; }

        public AppException(Error error) : base(error.Description) => Error = error;
    }
}
