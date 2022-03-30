namespace TomTom.Useful.EventSourcing
{
    public interface IAggregate<TIdentity> 
    {
        long Version { get; set; }
        IEnumerable<Event<TIdentity>> GetUncommitedEvents();
        void CleanUncommitedEvents();
    }

    public class AggregateBase<TIdentity> : IAggregate<TIdentity>
    {
        internal readonly List<Event<TIdentity>> _uncommitedEvents = new List<Event<TIdentity>>();
        public long Version { get; set; }

        public void CleanUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }

        public IEnumerable<Event<TIdentity>> GetUncommitedEvents()
        {
            return _uncommitedEvents;
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
            aggregate._uncommitedEvents.Add(@event);
        }
    }
}
