using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.EventSourcing
{
    public class Event : IMessage
    {
        public Event()
        {

        }

        protected Event(long sourceAggregateVersion, string causedById, string correlationId)
        {
            SourceAggregateVersion = sourceAggregateVersion;
            CausedById = causedById;
            CorrelationId = correlationId;
            Id = Guid.NewGuid();
        }

        public long SourceAggregateVersion { get; }
        public Guid Id { get; }

        public string CorrelationId { get; }
        public string CausedById { get; }
    }

    public class Event<TSourceAggregateId> : Event
    {
        protected Event(TSourceAggregateId sourceAggregateId, long sourceAggregateVersion, string causedById, string correlationId)
            : base(sourceAggregateVersion, causedById, correlationId)
        {
            SourceAggregateId = sourceAggregateId;
        }

        protected Event(ICommand<TSourceAggregateId> sourceCommand, long sourceAggregateVersion) 
            : this(sourceCommand.TargetIdentity, sourceAggregateVersion, sourceCommand.Id.ToString(), sourceCommand.CorrelationId)
        {

        }

        public TSourceAggregateId SourceAggregateId { get; }
    }
}
