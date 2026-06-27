namespace ProgressService.Shared
{
    public class Result
    {
        protected readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count() == 0;
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;


        //Success
        protected Result()
        {

        }

        //Fail With Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }

        // Fail with Errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static Result OK() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Can not Access Value Of Failed Result");

        //Success
        private Result(TValue value)
        {
            _value = value;
        }

        //Fail With Error
        private Result(Error error) : base(error)
        {
            _value = default!;
        }

        // Fail with Errors
        protected Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static Result<TValue> OK(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

        public static implicit operator Result<TValue>(TValue value) => OK(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);

    }
}
