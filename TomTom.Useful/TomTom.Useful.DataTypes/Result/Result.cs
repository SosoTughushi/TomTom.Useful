using System.Threading.Tasks;

namespace TomTom.Useful.DataTypes
{

    //You can see idea behind these classes here:
    //http://enterprisecraftsmanship.com/2015/03/20/functional-c-handling-failures-input-errors/

    public class Result
    {
        public bool Success { get; }
        public Result(bool success)
        {
            Success = success;
        }

        public static Result Ok() => new Result(true);

        public static Result<TError> Ok<TError>()//lift
        {
            return new Result<TError>();
        }

        public static Result<TError> Fail<TError>(TError error)//lift
        {
            return new Result<TError>(error);
        }
        public static Task<Result<TError>> FailAsync<TError>(TError error)//lift
        {
            return Task.FromResult(new Result<TError>(error));
        }

        public static Result<T, TError> Ok<T, TError>(T value)//lift
        {
            return new Result<T, TError>(value);
        }

        public static Result<T, TError> Fail<T, TError>(TError error)//lift
        {
            return new Result<T, TError>(error);
        }


        public static Task<Result<T, TError>> FailAsync<T, TError>(TError error)//lift
        {
            return Task.FromResult(new Result<T, TError>(error));
        }

        public static ResultFactory<TError> GetFactory<TError>() => Result<TError>.Factory;

    }

    public class Result<TError> : Result
    {
        public TError? Error { get; protected set; }

        /// <summary>
        /// success
        /// </summary>
        public Result() : base(true)
        {
        }

        /// <summary>
        /// fail
        /// </summary>
        public Result(TError error) : base(false)
        {
            Error = error;
        }


        public static ResultFactory<TError> Factory { get; } = new ResultFactory<TError>();

        public static implicit operator bool(Result<TError> err) => err.Success;
    }


    public class Result<T, TError> : Result<TError>
    {
        public T? Value { get; }
        /// <summary>
        /// success
        /// </summary>
        public Result(T value) : base()
        {
            Value = value;
        }

        /// <summary>
        /// fail
        /// </summary>
        public Result(TError error) : base(error)
        {
            Error = error;
            Value = default(T);
        }

        public static implicit operator bool(Result<T, TError> err) => err.Success;
    }
}
