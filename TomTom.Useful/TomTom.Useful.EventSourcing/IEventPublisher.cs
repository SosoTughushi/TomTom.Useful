using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public interface IEventPublisher<TIdentity> : IPublisher<Event<TIdentity>>
    {
    }

    public static class EventPublisherExtensions
    {
        public static IEventPublisher<TIdentity> AsEventPublisher<TIdentity>(this IPublisher<Event<TIdentity>> publisher)
        {
            return new EventPublisherAdapter<TIdentity>(publisher);
        }

        private class EventPublisherAdapter<TIdentity> : IEventPublisher<TIdentity>
        {
            private readonly IPublisher<Event<TIdentity>> publisher;

            public EventPublisherAdapter(IPublisher<Event<TIdentity>> publisher)
            {
                this.publisher = publisher;
            }
            public Task Publish(Event<TIdentity> message)
            {
                return publisher.Publish(message);
            }

            public Task Publish(IEnumerable<Event<TIdentity>> messages)
            {
                return publisher.Publish(messages);
            }
        }
    }
}
