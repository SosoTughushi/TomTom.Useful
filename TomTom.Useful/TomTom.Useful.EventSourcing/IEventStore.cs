using TomTom.Useful.DataTypes;

namespace TomTom.Useful.EventSourcing
{
    public interface IEventStore<TTargetAggregateIdentity>
    {
        Task<IEnumerable<Event<TTargetAggregateIdentity>>> GetEventsOfAggregate(TTargetAggregateIdentity identity, long offset = -1);

        Task<SaveEventsResult> SaveEvents(TTargetAggregateIdentity identity, IEnumerable<Event<TTargetAggregateIdentity>> events);
    }

    public class SaveEventsResult : Result<SaveEventsFailureReason>
    {
        public SaveEventsResult()
        {
        }

        public SaveEventsResult(SaveEventsFailureReason error) : base(error)
        {
        }
    }

    public enum SaveEventsFailureReason
    {
        EventVersionConflicts,
        Unknown
    }
}
