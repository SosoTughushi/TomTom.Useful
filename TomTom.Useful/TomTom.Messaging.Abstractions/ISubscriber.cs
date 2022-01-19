using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging
{
    public interface ISubscriber<T> where T : IMessage
    {
        Task<IAsyncDisposable> Subscribe(Func<T, ICurrentMessageContext, Task> handler);
    }

    public interface ICurrentMessageContext
    {
        Task Ack();
        Task Nack(object butWhy); // TODO: retry or permanently
    }

    public static class SubscriberExtensions
    {
        public static Task<IAsyncDisposable> Subscribe<T>(this ISubscriber<T> subscriber, Func<T, Task> handler) where T: IMessage
        {
            return subscriber.Subscribe(async (message, context) =>
            {
                try
                {
                    await handler(message);
                }
                catch (Exception ex)
                {
                    await context.Nack(ex);
                    throw;
                }
            });
        }
    }
}
