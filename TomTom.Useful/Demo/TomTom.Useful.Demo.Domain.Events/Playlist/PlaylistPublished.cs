using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public class PlaylistPublished : PlaylistEventBase
    {
        public PlaylistPublished(DateTime publishTimestamp)
        {
            this.PublishTimestamp = publishTimestamp;
        }

        public DateTime PublishTimestamp { get; }
    }
}
