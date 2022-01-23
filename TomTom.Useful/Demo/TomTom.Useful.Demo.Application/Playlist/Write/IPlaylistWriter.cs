using TomTom.Useful.Demo.Domain.Playlist.CommandHandlers;

namespace TomTom.Useful.Demo.Application.Playlist
{
    public interface IPlaylistWriter
    {
        Task<CreatePlaylistResult> Create(string title, DemoRequestContext context);

        Task<PublishPlaylistResult> Publish(Guid playlistId, DemoRequestContext context);
    }
}
