using System.Threading.Tasks;

namespace TomTom.Useful.DataTypes
{

    //You can see idea behind these classes here:
    //http://enterprisecraftsmanship.com/2015/03/20/functional-c-handling-failures-input-errors/

    public struct Result
    {
        public bool Success;
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

        public static ResultFactory<TError> GetBuilder<TError>() => Result<TError>.Builder;

    }

    public class Result<TError>
    {
        public bool Success;
        public TError Error;

        /// <summary>
        /// success
        /// </summary>
        public Result()
        {
            Success = true;
        }

        /// <summary>
        /// fail
        /// </summary>
        public Result(TError error)
        {
            Error = error;
            Success = false;
        }


        public static ResultFactory<TError> Builder { get; } = new ResultFactory<TError>();

        public static implicit operator Result(Result<TError> err)
        {
            return new Result(err.Success);
        }

        public static implicit operator bool(Result<TError> err) => err.Success;
    }


    public class Result<T, TError>
    {
        public TError Error;
        public bool Success;
        public T Value;
        /// <summary>
        /// success
        /// </summary>
        public Result(T value)
        {
            Value = value;
            Success = true;
            Error = default(TError);
        }

        /// <summary>
        /// fail
        /// </summary>
        public Result(TError error)
        {
            Error = error;
            Value = default(T);
            Success = false;
        }


        public static implicit operator Result<TError>(Result<T, TError> err)
        {
            if (err.Success)
            {
                return new Result<TError>();
            }
            return new Result<TError>(err.Error);
        }

        public static implicit operator Result<T, TError>(Result<TError> err)
        {
            if (err.Success)
            {
                return new Result<T, TError>(default(T));
            }

            return new Result<T, TError>(err.Error);

        }
        public static implicit operator bool(Result<T, TError> err) => err.Success;
    }
}
