using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public interface IEventPublisher : IPublisher<Event>
    {
    }

    public static class EventPublisherExtensions
    {
        public static IEventPublisher AsEventPublisher(this IPublisher<Event> publisher)
        {
            return new EventPublisherAdapter(publisher);
        }

        private class EventPublisherAdapter : IEventPublisher
        {
            private readonly IPublisher<Event> publisher;

            public EventPublisherAdapter(IPublisher<Event> publisher)
            {
                this.publisher = publisher;
            }
            public Task Publish(Event message)
            {
                return publisher.Publish(message);
            }

            public Task Publish(IEnumerable<Event> messages)
            {
                return publisher.Publish(messages);
            }
        }
    }
}
