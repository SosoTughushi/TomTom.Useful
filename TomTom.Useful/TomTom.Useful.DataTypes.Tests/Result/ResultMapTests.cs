using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TomTom.Useful.DataTypes.Tests.Result
{
    public class ResultMapTests
    {
        [Fact]
        public void Map_should_convert_successes()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var successResult = factory.Ok("18");

            // act
            var result =
                successResult.Map(str => int.Parse(str));

            // assert
            Assert.True(result.Success);
            Assert.Equal(18, result.Value);
        }

        [Fact]
        public void Map_should_chain_successes()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();
            var number = 159;

            var successResult = factory.Ok(number);

            // act
            var result =
                successResult
                .Map(c => c.ToString())
                .Map(int.Parse)
                .Map(c => c * c)
                .Map(c => c.ToString())
                .Map(int.Parse);

            // assert
            Assert.Equal(number * number, result.Value);
        }

        [Fact]
        public void Map_should_not_chain_if_initial_contains_fail()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var errorResult = factory.Fail<int>(ErrorTypes1.Error);

            // act
            var chainedCount = 0;
            var result =
                errorResult
                .Map(a =>
                {
                    chainedCount++;
                    return a.ToString();
                })
                .Map(a =>
                {
                    chainedCount++;
                    return int.Parse(a);
                });

            // assert
            Assert.False(result.Success);
            Assert.Equal(errorResult.Error, result.Error);
            Assert.Equal(0, chainedCount);
        }

        [Fact]
        public void MapFail_should_convert_fail()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var failResult = factory.Fail<int>(ErrorTypes1.ThirdError);

            // act
            var newResult = failResult.MapFail(c => ErrorTypes2.ThirdError);

            // assert
            Assert.False(newResult.Success);
            Assert.Equal(ErrorTypes2.ThirdError, newResult.Error);
        }

        [Fact]
        public void MapFail_should_not_be_called_on_success()
        {
            // arrange
            var factory = new ResultFactory<ErrorTypes1>();

            var successResult = factory.Ok(69);

            // act
            var failMapCount = 0;

            var newResult =
                successResult
                .MapFail(c =>
                {
                    failMapCount++;
                    return ErrorTypes2.ThirdError;
                })
                .MapFail(c =>
                {
                    failMapCount++;
                    return ErrorTypes3.FifthError;
                });

            // assert
            Assert.True(newResult.Success);
            Assert.Equal(0, failMapCount);
            Assert.Equal(successResult.Value, newResult.Value);
        }
    }
}
