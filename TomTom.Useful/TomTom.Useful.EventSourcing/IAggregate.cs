namespace TomTom.Useful.EventSourcing
{
    public interface IAggregate<TIdentity> 
    {
        long Version { get; set; }
        IEnumerable<Event<TIdentity>> GetUncommittedEvents();
        void CleanUncommittedEvents();
    }

    public class AggregateBase<TIdentity> : IAggregate<TIdentity>
    {
        internal readonly List<Event<TIdentity>> _uncommittedEvents = new List<Event<TIdentity>>();
        public long Version { get; set; }

        public void CleanUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public IEnumerable<Event<TIdentity>> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }
    }

    public static class AggregateExtensions
    {
        public static void EmitEvent<TAggregate, TIdentity, TEvent>(this TAggregate aggregate, TEvent @event)
            where TEvent : Event<TIdentity>
            where TAggregate : AggregateBase<TIdentity> , IEmitsEvent<TEvent>
        {
            aggregate.Version++;
            aggregate.Apply(@event);
            aggregate._uncommittedEvents.Add(@event);
        }
    }
}
