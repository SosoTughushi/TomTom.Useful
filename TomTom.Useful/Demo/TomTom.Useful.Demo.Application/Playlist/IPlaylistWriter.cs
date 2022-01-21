namespace TomTom.Useful.Demo.Application
{
    public interface IPlaylistWriter
    {
        Task<ResponseAddress> Create(string title, DemoAppContext context);

        Task<ResponseAddress> Publish(Guid playlistId, DemoAppContext context);
    }
}
