
namespace TomTom.Useful.Demo.Application.Playlist
{
    public interface IPlaylistReader
    {
        Task<PlaylistDto?> Get(Guid playlistId, DemoRequestContext context);
        Task<List<PlaylistDto>> GetAll(DemoRequestContext context);
    }
}