using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing.CommandHandling;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class HandleCreatePlaylist : ICreateAggregateCommandHandler<PlaylistIdentity, Playlist, CreatePlaylistCommand, PlaylistCommandRejectionReason>
    {
        private readonly IFilteredListProvider<Playlist> playlistProvider;

        public HandleCreatePlaylist(IFilteredListProvider<Playlist> playlistProvider)
        {
            this.playlistProvider = playlistProvider;
        }

        public async Task<CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandRejectionReason>> Handle(CreatePlaylistCommand command)
        {
            if (await TitleAlreadyExists(command.ExplorerId, command.Title))
            {
                return new CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandRejectionReason>(PlaylistCommandRejectionReason.PlaylistWithSameTitleAlreadyExists);
            }

            var playlist = new Playlist();
            var events = playlist.Create(command).ToList();
            return new CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandRejectionReason>(playlist, events);
        }

        private async Task<bool> TitleAlreadyExists(ExplorerIdentity owner, string title)
        {
            var items = await playlistProvider.GetFiltered(playlist => playlist.ExplorerId.Value == owner.Value && playlist.Title == title);

            if (items.Any())
            {
                return true;
            }

            return false;
        }
    }
}
