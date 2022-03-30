namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public class PublishPlaylistCommand : CommandBase<PublishPlaylistResult>
    {
        public PublishPlaylistCommand(Guid playlistId, Guid id, string causedById, string correlationId) 
            : base( id, causedById, correlationId)
        {
            PlaylistId = playlistId;
        }

        public Guid PlaylistId { get; }
    }
}
