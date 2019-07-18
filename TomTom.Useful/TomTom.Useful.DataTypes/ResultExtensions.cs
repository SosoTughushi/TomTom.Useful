using System;

namespace TomTom.Useful.DataTypes
{
    public static class ResultExtensions
    {

        #region this Result<T> => Result
        //bind with builder
        public static Result<TError> OnSuccess<T, TError>
            (this Result<T, TError> source, Func<T, ResultBuilder<TError>, Result<TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return res;
            return successFunc(res.Value, Result<TError>.Builder);
        }

        //map
        public static Result<TError> OnSuccess<T, TError>
            (this Result<T, TError> source, Action<T> successFunc)
        {
            return source.OnSuccess((prevResult, builder) =>
            {
                successFunc(prevResult);
                return builder.Ok();
            });
        }
        #endregion

        #region this Result<T> => Result<TResult>

        //bind with builder
        public static Result<TResult, TError> OnSuccess<T, TError, TResult>
            (this Result<T, TError> source, Func<T, ResultBuilder<TError>, Result<TResult, TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return Result.Fail<TResult, TError>(res.Error);
            return successFunc(res.Value, Result<TError>.Builder);
        }

        //map
        public static Result<TResult, TError> OnSuccess<T, TError, TResult>
            (this Result<T, TError> source, Func<T, TResult> successFunc)
        {
            return source.OnSuccess((prevResult, builder) =>
            {
                var newResult = successFunc(prevResult);
                return builder.Ok(newResult);
            });
        }

        #endregion

        #region this Result => Result
        //bind with builder
        public static Result<TError> OnSuccess<TError>
            (this Result<TError> source, Func<ResultBuilder<TError>, Result<TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return res;
            return successFunc(Result<TError>.Builder);
        }

        //map
        public static Result<TError> OnSuccess<TError>
            (this Result<TError> source, Action successFunc)
        {
            return source.OnSuccess(builder =>
            {
                successFunc();
                return builder.Ok();
            });
        }
        #endregion

        #region this Result => Result<T>

        //bind with builder
        public static Result<TResult, TError> OnSuccess<TError, TResult>
            (this Result<TError> source, Func<ResultBuilder<TError>, Result<TResult, TError>> successFunc)
        {
            var res = source;
            if (!res.Success)
                return Result.Fail<TResult, TError>(res.Error);
            return successFunc(Result<TError>.Builder);
        }

        //map
        public static Result<TResult, TError> OnSuccess<TError, TResult>
            (this Result<TError> source, Func<TResult> successFunc)
        {
            return source.OnSuccess((builder) =>
            {
                var newResult = successFunc();
                return builder.Ok(newResult);
            });
        }
        #endregion


        #region ChangeErrorType
        public static Result<TNewError> ChangeErrorType<TError, TNewError>
            (this Result<TError> source, Func<TError, TNewError> errorTypeConverter)
        {
            var result = source;
            if (result.Success)
                return Result.Ok<TNewError>();

            return Result.Fail(errorTypeConverter(result.Error));
        }

        public static Result<T, TNewError> ChangeErrorType<T, TError, TNewError>
            (this Result<T, TError> source, Func<TError, TNewError> errorTypeConverter)
        {
            var result = source;
            if (result.Success)
                return Result.Ok<T, TNewError>(result.Value);

            return Result.Fail<T, TNewError>(errorTypeConverter(result.Error));
        }
        #endregion

        #region OnError

        //bind
        public static Result<TError> OnError<TError>(this Result<TError> source, Func<TError, ResultBuilder<TError>, Result<TError>> onError)
        {
            var result = source;

            if (!result.Success)
            {
                return onError(result.Error, Result<TError>.Builder);
            }
            return result;
        }
        //map
        public static Result<TError> OnError<TError>(this Result<TError> source, Action<TError> onError)
        {
            var result = source;

            if (!result.Success)
            {
                onError(result.Error);
            }
            return result;
        }



        //map
        public static Result<T, TError> OnError<T, TError>(this Result<T, TError> source, Action<TError> onError)
        {
            var result = source;

            if (!result.Success)
            {
                onError(result.Error);
            }
            return result;
        }

        //bind
        public static Result<T, TError> OnError<T, TError>(this Result<T, TError> source, Func<TError, ResultBuilder<TError>, Result<T, TError>> onError)
        {
            var result = source;

            if (!result.Success)
            {
                return onError(result.Error, Result<TError>.Builder);
            }
            return result;
        }
        #endregion


        #region Handle Exception

        public static Result<TError> HandleException<TError, TException>(this Result<TError> source, Func<TException, Result<TError>> handle)
            where TException : Exception
        {
            try
            {
                return source;
            }
            catch (TException ex)
            {
                return handle(ex);
            }
        }


        public static Result<T, TError> HandleException<T, TError, TException>(this Result<T, TError> source, Func<TException, Result<T, TError>> handle)
            where TException : Exception
        {
            try
            {
                return source;
            }
            catch (TException ex)
            {
                return handle(ex);
            }
        }


        public static Result<T, TError> HandleException<T, TError, TException>(this Result<T, TError> source, Func<TException, TError> handle)
            where TException : Exception
        {
            try
            {
                return source;
            }
            catch (TException ex)
            {
                return Result.Fail<T, TError>(handle(ex));
            }
        }


        public static Result<T, TError> HandleException<T, TError, TException>(this Result<T, TError> source, Func<TException, T> handle)
            where TException : Exception
        {
            try
            {
                return source;
            }
            catch (TException ex)
            {
                return Result.Ok<T, TError>(handle(ex));
            }
        }

        #endregion
    }
}
