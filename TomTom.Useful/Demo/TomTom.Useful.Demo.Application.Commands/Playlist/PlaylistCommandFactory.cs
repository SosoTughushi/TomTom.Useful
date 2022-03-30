namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public static class PlaylistCommandFactory
    {
        public static CreatePlaylistCommand Create(string title, DemoRequestContext context)
        {
            return new CreatePlaylistCommand(
                messageId: Guid.NewGuid(),
                context.CurrentUserId,
                creationTimestamp: DateTime.UtcNow,
                title: title,
                causedById: context.RequestId,
                correlationId: context.CorrelationId);
        }

        public static PublishPlaylistCommand PublishPlaylist(Guid playlistId, DemoRequestContext context)
        {
            return new PublishPlaylistCommand(
                playlistId,
                Guid.NewGuid(),
                context.RequestId,
                context.CorrelationId);
        }
    }
}
