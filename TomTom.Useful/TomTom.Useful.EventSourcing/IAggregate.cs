using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.EventSourcing
{
    public interface IAggregate
    {
        long Version { get; set; }
    }
    public interface IAggregate<TIdentity> : IAggregate
    {
        TIdentity Id { get; }
    }

    public static class AggregateExtensions
    {
        public static TEvent EmitEvent<TAggregate, TEvent>(this TAggregate aggregate, TEvent @event)
            where TEvent : Event
            where TAggregate : IAggregate, IEmitsEvent<TEvent>
        {
            aggregate.Version++;
            aggregate.Apply(@event);
            return @event;
        }
    }
}
