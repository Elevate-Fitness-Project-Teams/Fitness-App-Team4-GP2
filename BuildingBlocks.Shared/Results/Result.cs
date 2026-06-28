namespace BuildingBlocks.Shared.Results
{
    /// <summary>
    /// A functional result used for control flow inside the application layer:
    /// success carries no error, failure carries one or more <see cref="Error"/>s.
    /// Map it to the wire envelope at the boundary via <c>ToApiResponse</c>.
    /// </summary>
    public class Result
    {
        protected readonly List<Error> _errors = new();

        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;

        protected Result() { }

        protected Result(Error error) => _errors.Add(error);

        protected Result(IEnumerable<Error> errors) => _errors.AddRange(errors);

        public static Result OK() => new();
        public static Result Fail(Error error) => new(error);
        public static Result Fail(IEnumerable<Error> errors) => new(errors);

        public static implicit operator Result(Error error) => Fail(error);
    }

    /// <summary>A <see cref="Result"/> that carries a value on success.</summary>
    public sealed class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(TValue value) => _value = value;

        private Result(Error error) : base(error) => _value = default!;

        private Result(IEnumerable<Error> errors) : base(errors) => _value = default!;

        public static Result<TValue> OK(TValue value) => new(value);
        public static new Result<TValue> Fail(Error error) => new(error);
        public static new Result<TValue> Fail(IEnumerable<Error> errors) => new(errors);

        public static implicit operator Result<TValue>(TValue value) => OK(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
    }
}
