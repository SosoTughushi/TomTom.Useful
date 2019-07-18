using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.DataTypes
{
    public class ResultBuilder<TError>
    {
        public Result<TError> Ok()//lift
        {
            return new Result<TError>();
        }

        public Task<Result<TError>> OkAsync() => Task.FromResult(Ok());

        public Result<TError> Fail(TError error)//lift
        {
            return new Result<TError>(error);
        }

        public Task<Result<TError>> FailAsync(TError error) => Task.FromResult(Fail(error));

        public Result<T, TError> Ok<T>(T value)//lift
        {
            return new Result<T, TError>(value);
        }
        public Task<Result<T, TError>> OkAsync<T>(T value) => Task.FromResult(Ok(value));

        public Result<T, TError> Fail<T>(TError error)//lift
        {
            return new Result<T, TError>(error);
        }

        public Task<Result<T, TError>> FailAsync<T>(TError error) => Task.FromResult(Fail<T>(error));
    }
}
