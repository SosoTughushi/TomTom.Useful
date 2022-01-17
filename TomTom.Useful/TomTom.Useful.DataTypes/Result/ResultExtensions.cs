using System;

namespace TomTom.Useful.DataTypes
{
    public static class ResultExtensions
    {

        #region this Result<T> => Result
        //bind with builder
        public static Result<TError> Bind<T, TError>
            (this Result<T, TError> source, Func<T, ResultFactory<TError>, Result<TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return res;
            return successFunc(res.Value, Result<TError>.Factory);
        }

        public static Result<TError> OnSuccess<T, TError>
            (this Result<T, TError> source, Action<T> successFunc)
        {
            return source.Bind((prevResult, builder) =>
            {
                successFunc(prevResult);
                return builder.Ok();
            });
        }
        #endregion

        #region this Result<T,TError> => Result<T,TResult>

        //bind with builder
        public static Result<TResult, TError> Bind<T, TError, TResult>
            (this Result<T, TError> source, Func<T, ResultFactory<TError>, Result<TResult, TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return Result.Fail<TResult, TError>(res.Error);
            return successFunc(res.Value, Result<TError>.Factory);
        }

        public static Result<TResult, TError> Map<T, TError, TResult>
            (this Result<T, TError> source, Func<T, TResult> successFunc)
        {
            return source.Bind((prevResult, builder) =>
            {
                var newResult = successFunc(prevResult);
                return builder.Ok(newResult);
            });
        }

        #endregion

        #region this Result => Result
        //bind with builder
        public static Result<TError> Bind<TError>
            (this Result<TError> source, Func<ResultFactory<TError>, Result<TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return res;
            return successFunc(Result<TError>.Factory);
        }

        public static Result<TError> Map<TError>
            (this Result<TError> source, Action successFunc)
        {
            return source.Bind(builder =>
            {
                successFunc();
                return builder.Ok();
            });
        }
        #endregion

        #region this Result => Result<T>

        //bind with builder
        public static Result<TResult, TError> Bind<TError, TResult>
            (this Result<TError> source, Func<ResultFactory<TError>, Result<TResult, TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return Result.Fail<TResult, TError>(res.Error);

            return successFunc(Result<TError>.Factory);
        }

        public static Result<TResult, TError> Map<TError, TResult>
            (this Result<TError> source, Func<TResult> successFunc)
        {
            return source.Bind((builder) =>
            {
                var newResult = successFunc();
                return builder.Ok(newResult);
            });
        }
        #endregion


        #region OnFail
        
        public static Result<TError> OnFail<TError>(this Result<TError> source, Action<TError> onError)
        {
            var result = source;

            if (!result.Success)
            {
                onError(result.Error);
            }

            return result;
        }

        public static Result<T, TNewError> MapFail<T, TError, TNewError>(
            this Result<T, TError> source, 
            Func<TError, TNewError> errorConverter)
        {
            var result = source;

            if (!result.Success)
            {
                return new Result<T,TNewError>(errorConverter(result.Error));
            }

            return Result<TNewError>.Factory.Ok(result.Value);
        }

        public static Result< TNewError> MapFail<TError, TNewError>(
            this Result<TError> source,
            Func<TError, TNewError> errorConverter)
        {
            var result = source;

            if (!result.Success)
            {
                return Result<TNewError>.Factory.Fail(errorConverter(result.Error));
            }

            return Result<TNewError>.Factory.Ok();
        }


        #endregion
    }
}
