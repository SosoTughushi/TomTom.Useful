namespace TomTom.Useful.Demo.Application.Queries.Playlists
{
    public class GetPlaylists : QueryBase<List<PlaylistDto>>
    {
        public GetPlaylists(DemoRequestContext context)
            : base(context)
        {
        }
    }
}
