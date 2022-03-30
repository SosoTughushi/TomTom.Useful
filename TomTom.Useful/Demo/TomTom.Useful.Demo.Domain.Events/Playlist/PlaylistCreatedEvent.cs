using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public class PlaylistCreatedEvent : PlaylistEventBase
    {
        public PlaylistCreatedEvent(string title, ExplorerIdentity explorerId, DateTime creationTimestamp)
        {
            Title = title;
            ExplorerId = explorerId;
            CreationTimestamp = creationTimestamp;
        }

        public string Title { get; }
        public ExplorerIdentity ExplorerId { get; }
        public DateTime CreationTimestamp { get; }
    }
}
