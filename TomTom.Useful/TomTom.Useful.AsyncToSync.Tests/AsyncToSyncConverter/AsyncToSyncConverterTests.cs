using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TomTom.Useful.AsyncToSync.Tests.AsyncToSyncConverter
{
    public class AsyncToSyncConverterTests
    {
        [Fact]
        public async Task Should_return_data()
        {
            // arrange
            var converter = new Sut(2000);
            var item = new Something
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            // act
            var resultTask = converter.AwaitResult(item.CorrelationId);
            converter.SetResult(item);
            var result = await resultTask;

            // assert
            Assert.Equal(item.CorrelationId, result.CorrelationId);
        }

        [Fact]
        public async Task Should_be_thread_safe()
        {
            // arrange
            var items = Enumerable.Range(0, 500).Select(c => new Something
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Number = c
            }).ToList();

            var converter = new Sut(500);

            // act
            var awaiters = items.Select(c => converter.AwaitResult(c.CorrelationId)).ToArray();
            var allAwaiter = Task.WhenAll(awaiters);
            Parallel.ForEach(items, item =>
            {
                converter.SetResult(item);
            });

            await allAwaiter;

            // assert
            var receivedItems = awaiters.Select(c => c.Result).OrderBy(c => c.Number).ToList();

            Assert.Equal(items, receivedItems);
        }

        [Fact]
        public async Task Awaiting_two_times_should_result_in_InvalidOperationException()
        {
            // arrange
            var converter = new Sut();
            var item = new Something
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            // act
            var firstAwaiter = converter.AwaitResult(item.CorrelationId);
            await Assert.ThrowsAsync<InvalidOperationException>(() => converter.AwaitResult(item.CorrelationId));

            converter.SetResult(item);

            var firstResult = await firstAwaiter;

            // assert
            Assert.Equal(item, firstResult);
        }

        [Fact]
        public async Task DelayingAwait_should_result_in_timeout()
        {
            // arrange
            var converter = new Sut(200);
            var item = new Something
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            // act
            converter.SetResult(item);
            var awaiter = converter.AwaitResult(item.CorrelationId);

            // assert
            await Assert.ThrowsAsync<TimeoutException>(() => awaiter);
        }


        [Fact]
        public async Task Should_timeout_when_wait_pariod_is_high()
        {
            // arrange
            var converter = new Sut(500);
            var item = new Something
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Number = 15
            };

            // act
            var resultTask = converter.AwaitResult(item.CorrelationId);
            await Task.Delay(600).ContinueWith(t => converter.SetResult(item));

            // assert
            await Assert.ThrowsAsync<TimeoutException>(() => resultTask);
        }
    }
}
