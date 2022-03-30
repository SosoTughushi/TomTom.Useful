using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.EventSourcing
{
    public class EventSourcedAggregateRepository<TIdentity, TAggregate> : IEntityByKeyProvider<TIdentity, TAggregate>
        where TAggregate : IAggregate<TIdentity>, new()
    {
        private readonly IEventStore<TIdentity> eventRepository;

        public EventSourcedAggregateRepository(IEventStore<TIdentity> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<TAggregate> Get(TIdentity identity)
        {
            var events = await this.eventRepository.GetEventsOfAggregate(identity);

            var aggregate = new TAggregate();

            AggregateEventApplier<TIdentity, TAggregate>.ApplyEvents(aggregate, events);

            return aggregate;
        }

    }


}
