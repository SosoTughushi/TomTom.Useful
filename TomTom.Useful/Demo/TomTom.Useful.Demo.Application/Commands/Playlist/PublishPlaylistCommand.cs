namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public class PublishPlaylistCommand : CommandBase<PublishPlaylistResult>
    {
        public PublishPlaylistCommand(Guid playlistId, DemoRequestContext context) 
            : base(context)
        {
            PlaylistId = playlistId;
        }

        public Guid PlaylistId { get; }
    }
}
