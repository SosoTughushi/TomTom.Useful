using TomTom.Useful.Demo.Application.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.CommandHandlers
{
    public class PublishPlaylistHandler :
        ICommandHandler<PublishPlaylistCommand, PublishPlaylistResult>
    {
        private readonly IEntityByKeyProvider<PlaylistIdentity, Domain.Playlist.Playlist?> playlistByIdProvider;
        private readonly IUpdater<Domain.Playlist.Playlist> playlistUpdater;

        public PublishPlaylistHandler(
            IEntityByKeyProvider<PlaylistIdentity, Domain.Playlist.Playlist?> playlistByIdProvider
           , IUpdater<Domain.Playlist.Playlist> playlistUpdater)
        {
            this.playlistByIdProvider = playlistByIdProvider;
            this.playlistUpdater = playlistUpdater;
        }

        public async Task<PublishPlaylistResult> Handle(PublishPlaylistCommand command, CancellationToken cancellationToken)
        {
            var playlist = await this.playlistByIdProvider.Get(command.PlaylistId);
            if (playlist == null)
            {
                throw new InvalidOperationException($"Playlist with playlistId='{command.PlaylistId}' does not exist.");
            }

            var isPublished = playlist.Publish();

            if (!isPublished)
            {
                return new PublishPlaylistResult(PublishPlaylistFailureReason.AlreadyPublished);
            }

            var result = await this.playlistUpdater.Update(playlist);

            if (result)
            {
                return new PublishPlaylistResult();
            }

            return new PublishPlaylistResult(PublishPlaylistFailureReason.Unknown);
        }
    }
}
