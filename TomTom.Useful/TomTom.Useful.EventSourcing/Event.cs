using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public class Event : IMessage
    {

        public long SourceAggregateVersion { get; }
        public Guid MessageId { get; }

        public string CorrelationId { get; }
        public string CausedById { get; }
    }

    public class Event<TSourceAggregateId> : Event
    {
        public TSourceAggregateId SourceAggregateId { get; }
    }
}
