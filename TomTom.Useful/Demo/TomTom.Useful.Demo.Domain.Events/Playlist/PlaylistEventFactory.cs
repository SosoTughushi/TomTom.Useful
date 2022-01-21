using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public static class PlaylistEventFactory
    {
        public static PlaylistCreatedEvent PlaylistCreated(Guid causedByMessageId, string correlationId, string title, ExplorerIdentity explorerId, DateTime creationTimestamp)
        {
            return new PlaylistCreatedEvent(
                sourceAggregateId: Guid.NewGuid(),
                sourceAggregateVersion: 0,
                causedById: causedByMessageId.ToString(),
                correlationId: correlationId,
                title: title,
                explorerId: explorerId,
                creationTimestamp: DateTime.UtcNow);
        }

        public static PlaylistPublished PlaylistPublished(PlaylistIdentity playlistId, long sourceVersion, Guid causedByMessageId, string correlationId)
        {
            return new PlaylistPublished(playlistId, sourceVersion, causedByMessageId.ToString(), correlationId, DateTime.UtcNow);
        }
    }
}
