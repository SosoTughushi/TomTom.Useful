using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.EventSourcing
{
    public class EventSourcedAggregateRepository<TIdentity, TAggregate> : IEntityByKeyProvider<TIdentity, TAggregate>
        where TAggregate : IAggregate<TIdentity>, new()
    {
        private readonly IEventRepository<TIdentity> eventRepository;

        public EventSourcedAggregateRepository(IEventRepository<TIdentity> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<TAggregate> Get(TIdentity identity)
        {
            var events = await this.eventRepository.GetEventsOfAggregate(identity);

            var aggregate = new TAggregate();

            AggregateEventApplier<TAggregate>.ApplyEvents(aggregate, events);

            return aggregate;
        }

    }


}
