using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public static class PlaylistEventFactory
    {
        public static PlaylistCreatedEvent PlaylistCreated(string title, ExplorerIdentity explorerId, DateTime creationTimestamp)
        {
            return new PlaylistCreatedEvent(
                title: title,
                explorerId: explorerId,
                creationTimestamp: DateTime.UtcNow);
        }

        public static PlaylistPublished PlaylistPublished()
        {
            return new PlaylistPublished(DateTime.UtcNow);
        }
    }
}
