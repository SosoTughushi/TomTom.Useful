using Microsoft.Extensions.Logging;
using TomTom.Useful.Serializers.Json;

namespace TomTom.Useful.Messaging.InMemory
{
    public class InMemoryMessageBus<T> : ISubscriber<T>, IPublisher<T>
        where T : IMessage
    {
        public InMemoryMessageBus(
            InMemoryBusSettings options,
            ILogger<InMemoryMessageBus<T>> logger,
            JsonSerializer<T> jsonSerializer)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        private readonly Dictionary<Guid, Func<T, ICurrentMessageContext, Task>> subscriptions = new Dictionary<Guid, Func<T, ICurrentMessageContext, Task>>();
        private readonly InMemoryBusSettings options;
        private readonly ILogger<InMemoryMessageBus<T>> logger;
        private readonly JsonSerializer<T> jsonSerializer;

        public async Task Publish(T message)
        {
            foreach (var subsubscription in subscriptions.Values)
            {
                try
                {
                    var context = new Context();
                    await subsubscription(message, context);
                }
                catch (Exception ex)
                {
                    if (this.options.LogWholeMessageOnFault)
                    {
                        this.logger.LogError(ex,
                            "Error while processing message: {0}",
                            this.jsonSerializer.SerializeToString(message));
                    }
                    else
                    {
                        this.logger.LogError(ex,
                            "Error while processing message of type {0}. Id='{1}', CorrelationId='{2}', CausationId='{3}'",
                            message.GetType().FullName,
                            message.Id,
                            message.CorrelationId,
                            message.CausationId);
                    }
                }
            }
        }

        public async Task Publish(IEnumerable<T> messages)
        {
            foreach (var message in messages)
            {
                await Publish(message);
            }
        }

        public Task<IAsyncDisposable> Subscribe(Func<T, ICurrentMessageContext, Task> handler)
        {
            var id = Guid.NewGuid();
            this.subscriptions.Add(id, handler);

            return Task.FromResult<IAsyncDisposable>(new Subscription(() => this.subscriptions.Remove(id)));
        }

        private class Subscription : IAsyncDisposable
        {
            private readonly Action onDispose;

            public Subscription(Action onDispose)
            {
                this.onDispose = onDispose;
            }
            public ValueTask DisposeAsync()
            {
                this.onDispose();
                return ValueTask.CompletedTask;
            }
        }

        private class Context : ICurrentMessageContext
        {
            public Task Ack()
            {
                return Task.CompletedTask;
            }

            public Task Nack(object butWhy)
            {
                return Task.CompletedTask;
            }
        }
    }

    public class InMemoryBusSettings
    {
        public bool LogWholeMessageOnFault { get; set; }
    }
}
