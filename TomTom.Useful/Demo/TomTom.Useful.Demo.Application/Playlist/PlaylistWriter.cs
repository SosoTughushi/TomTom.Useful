using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Application
{
    public class PlaylistWriter : IPlaylistWriter
    {
        private readonly ICommandPublisher<ICommand<PlaylistIdentity>> publisher;

        public PlaylistWriter(ICommandPublisher<ICommand<PlaylistIdentity>> publisher)
        {
            this.publisher = publisher;
        }
        public async Task<ResponseAddress> Create(string title, DemoAppContext context)
        {
            var command = PlaylistCommandFactory.Create(
                explorerId: context.CurrentUserId,
                title: title,
                causedById: context.RequestId,
                correlationId: context.CorrelationId
                );

            await publisher.Publish(command);

            return new ResponseAddress(command.Id);
        }

        public async Task<ResponseAddress> Publish(Guid playlistId, DemoAppContext context)
        {
            var command = PlaylistCommandFactory.PublishPlaylist(
                playlistId: playlistId,
                causedById: context.RequestId,
                correlationId: context.CorrelationId);

            await publisher.Publish(command);

            return new ResponseAddress(command.Id);
        }
    }
}
