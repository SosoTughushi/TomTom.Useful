using TomTom.Useful.Demo.Domain.Playlist.CommandHandlers;

namespace TomTom.Useful.Demo.Application
{
    public interface IPlaylistWriter
    {
        Task<CreatePlaylistResult> Create(string title, DemoAppContext context);

        Task<PublishPlaylistResult> Publish(Guid playlistId, DemoAppContext context);
    }
}
