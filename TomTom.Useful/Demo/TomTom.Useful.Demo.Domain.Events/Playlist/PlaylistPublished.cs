using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public class PlaylistPublished : Event<PlaylistIdentity>
    {
        public PlaylistPublished(PlaylistIdentity sourceAggregateId, long sourceAggregateVersion, string causedById, string correlationId, DateTime publishTimestamp) : base(sourceAggregateId, sourceAggregateVersion, causedById, correlationId)
        {
            this.PublishTimestamp = publishTimestamp;
        }

        public DateTime PublishTimestamp { get; }
    }
}
