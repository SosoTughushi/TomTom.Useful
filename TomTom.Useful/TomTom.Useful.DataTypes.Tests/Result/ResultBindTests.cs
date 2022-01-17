using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TomTom.Useful.DataTypes.Tests.Result
{
    public class ResultBindTests
    {
        [Fact]
        public void Bind_should_change_success_result_value()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var successResult = factory.Ok(15);

            // act
            var newResult = successResult.Bind((c, f) => f.Ok("18"));

            // assert
            Assert.Equal("18", newResult.Value);
        }

        [Fact]
        public void Bind_should_change_success_to_fail()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var successResult = factory.Ok(15);

            // act
            var newResult = successResult.Bind((c, f) => f.Fail<int>(ErrorTypes1.SecondError));

            // assert
            Assert.False(newResult);
            Assert.Equal(ErrorTypes1.SecondError, newResult.Error);
        }

        [Fact]
        public void Bind_should_chain_multiple_binds()
        {
            // arrange
            Result<int, ErrorTypes3> validate1(int value, ResultFactory<ErrorTypes3> factory)
            {
                return factory.Ok(value);
            }

            Result<int, ErrorTypes3> validate2(int value, ResultFactory<ErrorTypes3> factory)
            {
                return factory.Ok(value);
            }
            Result<int, ErrorTypes3> validate3(int value, ResultFactory<ErrorTypes3> factory)
            {
                return factory.Fail<int>(ErrorTypes3.FifthError);
            }

            var successResult = Result<ErrorTypes3>.Factory.Ok(15);

            // act
            var result = successResult
                .Bind((v, f) => validate1(v, f))
                .Bind((v, f) => validate2(v, f))
                .Bind((v, f) => validate3(v, f));

            // assert
            Assert.False(result);
            Assert.Equal(ErrorTypes3.FifthError, result.Error);
        }


        [Fact]
        public void Demon_stration()
        {
            Result<int, string> getNumberFromDb() =>
                DataTypes.Result.GetFactory<string>().Ok(new Random().Next(1, 500));

            var factory = DataTypes.Result.GetFactory<ErrorTypes4>();

            Result<int, ErrorTypes4> isOdd(int number)
            {
                if (number % 2 == 0)
                {
                    return factory.Ok(number);
                }
                return new Result<int, ErrorTypes4>(ErrorTypes4.NotOdd);
            }

            Result<int, ErrorTypes4> isPositive(int number)
            {
                if (number > 0)
                {
                    return factory.Ok(number);
                }

                return new Result<int, ErrorTypes4>(ErrorTypes4.Negative);
            }

            Result<int, ErrorTypes4> isDirty(int number)
            {
                if (number != 69)
                {
                    return factory.Ok(number);
                }
                return new Result<int, ErrorTypes4>(ErrorTypes4.Dirty);
            }

            getNumberFromDb()
                .MapFail(s => ErrorTypes4.Unknown)
                .Bind((number, _) => isOdd(number))
                .Bind((number, _) => isPositive(number))
                .Bind((number, _) => isDirty(number))
                .MapFail(error => new ComplexError(error, "something went wrong with number"))
                .Map(number => new { number });

        }
    }
}
