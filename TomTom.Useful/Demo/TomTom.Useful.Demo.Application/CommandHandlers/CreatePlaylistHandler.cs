using TomTom.Useful.Demo.Application.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.CommandHandlers
{
    public class CreatePlaylistHandler :
        ICommandHandler<CreatePlaylistCommand, CreatePlaylistResult>
    {

        private readonly IInserter<Domain.Playlist.Playlist> playlistInserter;
        private readonly IFilteredListProvider<Domain.Playlist.Playlist> playlistFilteredProvider;

        public CreatePlaylistHandler(IInserter<Domain.Playlist.Playlist> playlistInserter, IFilteredListProvider<Domain.Playlist.Playlist> playlistFilteredProvider)
        {
            this.playlistInserter = playlistInserter;
            this.playlistFilteredProvider = playlistFilteredProvider;
        }

        public async Task<CreatePlaylistResult> Handle(CreatePlaylistCommand command, CancellationToken cancellationToken)
        {

            if (await TitleAlreadyExists(command.ExplorerId, command.Title))
            {
                return new CreatePlaylistResult(CreatePlaylistFailureReason.TitleAlreadyExists);
            }

            var playlist = new Domain.Playlist.Playlist(command.Title, command.ExplorerId, command.CreationTimestamp);

            var result = await this.playlistInserter.Insert(playlist);

            if (result)
            {
                return new CreatePlaylistResult(playlist.Id);
            }

            return new CreatePlaylistResult(CreatePlaylistFailureReason.Unknown);
        }

        private async Task<bool> TitleAlreadyExists(ExplorerIdentity owner, string title)
        {
            var items = await playlistFilteredProvider.GetFiltered(playlist => playlist.ExplorerId.Value == owner.Value && playlist.Title == title);

            if (items.Any())
            {
                return true;
            }

            return false;
        }
    }
}
