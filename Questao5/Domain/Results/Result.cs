namespace Questao5.Domain.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Ok()
        {
            return new Result(true, Error.None);
        }

        public static Result Fail(Error error)
        {
            return new Result(false, error);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(true, value, Error.None);
        }

        public static Result<T> Ok<T>()
        {
            return new Result<T>(true, default, Error.None);
        }

        public static Result<T> Fail<T>(Error error)
        {
            return new Result<T>(false, default, error);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected internal Result(bool isSuccess, T value, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }
    }
}
