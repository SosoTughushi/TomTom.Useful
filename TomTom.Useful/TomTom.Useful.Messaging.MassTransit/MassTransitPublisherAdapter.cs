using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging.MassTransit
{
    public class MassTransitPublisherAdapter<T> : IPublisher<T>
        where T : class, IMessage
    {
        private readonly IBus bus;

        public MassTransitPublisherAdapter(IBus bus)
        {
            this.bus = bus;
        }
        public async Task Publish(T message)
        {
            await bus.Publish(message);
        }

        public async Task Publish(IEnumerable<T> messages)
        {
            await bus.PublishBatch<T>(messages);
        }
    }


    public class MassTransitConsumer<T> : IConsumer<T>
        where T : class, IMessage
    {
        private readonly Func<T, ICurrentMessageContext, Task> handler;

        public MassTransitConsumer(Func<T, ICurrentMessageContext, Task> handler)
        {
            this.handler = handler;
        }

        public Task Consume(ConsumeContext<T> context)
        {
            var adaptedContext = new MassTransitContextAdapter(context);
            return handler(context.Message, adaptedContext);
        }

        private class MassTransitContextAdapter : ICurrentMessageContext
        {
            private readonly ConsumeContext<T> massTransitContext;

            public MassTransitContextAdapter(ConsumeContext<T> massTransitContext)
            {
                this.massTransitContext = massTransitContext;
            }
            public Task Ack()
            {
                return Task.CompletedTask;
            }

            public Task Nack(object butWhy)
            {
                return Task.FromException(new MassTransitConsumerException(butWhy));
            }
        }
    }

    public class MassTransitConsumerException : Exception
    {
        public MassTransitConsumerException(object butWhy)
        {
            ButWhy = butWhy;
        }

        public object ButWhy { get; }
    }
}
