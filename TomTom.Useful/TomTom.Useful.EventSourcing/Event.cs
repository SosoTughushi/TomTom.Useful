using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public class Event : IMessage
    {
        protected Event(long sourceAggregateVersion, Guid causedById, Guid correlationId)
        {
            SourceAggregateVersion = sourceAggregateVersion;
            CausationId = causedById;
            CorrelationId = correlationId;
            Id = Guid.NewGuid();
        }

        public long SourceAggregateVersion { get; }
        public Guid? Id { get; }

        public Guid CorrelationId { get; }
        public Guid? CausationId { get; }
    }

    public class Event<TSourceAggregateId> : Event
    {
        protected Event(TSourceAggregateId sourceAggregateId, long sourceAggregateVersion, Guid causedById, Guid correlationId)
            : base(sourceAggregateVersion, causedById, correlationId)
        {
            SourceAggregateId = sourceAggregateId;
        }

        public TSourceAggregateId SourceAggregateId { get; }
    }
}
