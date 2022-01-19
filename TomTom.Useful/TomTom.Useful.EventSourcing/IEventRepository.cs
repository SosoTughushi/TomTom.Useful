using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.EventSourcing
{
    public interface IEventRepository<TTargetAggregateIdentity>
    {
        Task<IEnumerable<Event<TTargetAggregateIdentity>>> GetEventsOfAggregate(TTargetAggregateIdentity identity, long offset = -1);
    }
}
