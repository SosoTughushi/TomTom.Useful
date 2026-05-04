using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public class Event : IMessage
    {
        protected Event(long sourceAggregateVersion, string correlationId, string causedById)
        {
            SourceAggregateVersion = sourceAggregateVersion;
            MessageId = Guid.NewGuid();
            CorrelationId = correlationId;
            CausedById = causedById;
        }

        public long SourceAggregateVersion { get; }
        public Guid MessageId { get; }

        public string CorrelationId { get; }
        public string CausedById { get; }
    }

    public class Event<TSourceAggregateId> : Event
    {
        protected Event(TSourceAggregateId sourceAggregateId, long sourceAggregateVersion, string correlationId, string causedById)
            : base(sourceAggregateVersion, correlationId, causedById)
        {
            SourceAggregateId = sourceAggregateId;
        }

        public TSourceAggregateId SourceAggregateId { get; }
    }
}
