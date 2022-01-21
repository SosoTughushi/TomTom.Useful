using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public class PlaylistEventBase : Event<PlaylistIdentity>
    {
        protected PlaylistEventBase(
            PlaylistIdentity sourceAggregateId, 
            long sourceAggregateVersion, 
            string causedById,
            string correlationId) 
            : base(sourceAggregateId, sourceAggregateVersion, causedById, correlationId)
        {
        }

        
    }
}
