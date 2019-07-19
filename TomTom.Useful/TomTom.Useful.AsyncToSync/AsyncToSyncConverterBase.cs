using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TomTom.Useful.AsyncToSync
{
    public abstract class AsyncToSyncConverterBase<TKey, T>
    {
        private readonly int timeoutInMs;
        private readonly ConcurrentDictionary<TKey, Subscription> subscriptions;

        public AsyncToSyncConverterBase(int timeoutInMs)
        {
            this.subscriptions = new ConcurrentDictionary<TKey, Subscription>();
            this.timeoutInMs = timeoutInMs;
        }

        protected abstract TKey ExtractKey(T item);

        public Task<T> AwaitResult(TKey key)
        {
            var subscription = new Subscription();

            if(this.subscriptions.TryAdd(key, subscription))
            {
#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
                Task.Delay(this.timeoutInMs, subscription.CancellationTokenSource.Token)
                    .ContinueWith((x) =>
                    {
                        this.SetResult(key, source =>
                        {
                            source.SetException(new TimeoutException($"Awaiting result for <{typeof(T).Name}> with [{key}] timed out."));
                        });
                    });
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler

                return subscription.TaskCompletionSource.Task;
            }

            return Task.FromException<T>(new InvalidOperationException($"Already awaiting result for <{typeof(T).Name}> with [{key}]."));
        }

        public void SetResult(T item)
        {
            var key = this.ExtractKey(item);

            this.SetResult(key, tcs => tcs.SetResult(item));
        }

        private void SetResult(TKey key, Action<TaskCompletionSource<T>> resultSetter)
        {
            if (this.subscriptions.TryRemove(key, out var subscription))
            {
                if (!subscription.TaskCompletionSource.Task.IsCompleted)
                {
                    resultSetter(subscription.TaskCompletionSource);
                    subscription.CancellationTokenSource.Cancel();
                }
            }
        }

        private class Subscription
        {
            public Subscription()
            {
                this.TaskCompletionSource = new TaskCompletionSource<T>();
                this.CancellationTokenSource = new CancellationTokenSource();
            }

            public TaskCompletionSource<T> TaskCompletionSource { get; }

            public CancellationTokenSource CancellationTokenSource { get; }
        }
    }
}
