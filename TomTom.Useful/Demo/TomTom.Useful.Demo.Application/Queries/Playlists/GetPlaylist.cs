namespace TomTom.Useful.Demo.Application.Queries.Playlists
{
    public class GetPlaylist : QueryBase<PlaylistDto?>
    {
        public GetPlaylist(
            Guid playlistId,
            DemoRequestContext context) : base(context)
        {
            PlaylistId = playlistId;
        }

        public Guid PlaylistId { get; }
    }
}
