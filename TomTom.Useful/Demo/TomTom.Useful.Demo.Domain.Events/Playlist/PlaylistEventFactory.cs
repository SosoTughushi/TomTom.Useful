using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Events.Playlist
{
    public static class PlaylistEventFactory
    {
        public static PlaylistCreatedEvent PlaylistCreated(PlaylistIdentity sourceAggregateId, long sourceAggregateVersion, string correlationId, string causedById,
            string title, ExplorerIdentity explorerId, DateTime creationTimestamp)
        {
            return new PlaylistCreatedEvent(
                sourceAggregateId: sourceAggregateId,
                sourceAggregateVersion: sourceAggregateVersion,
                correlationId: correlationId,
                causedById: causedById,
                title: title,
                explorerId: explorerId,
                creationTimestamp: creationTimestamp);
        }

        public static PlaylistPublished PlaylistPublished(PlaylistIdentity sourceAggregateId, long sourceAggregateVersion, string correlationId, string causedById,
            DateTime publishTimestamp)
        {
            return new PlaylistPublished(
                sourceAggregateId: sourceAggregateId,
                sourceAggregateVersion: sourceAggregateVersion,
                correlationId: correlationId,
                causedById: causedById,
                publishTimestamp: publishTimestamp);
        }
    }
}
