using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing.CommandHandling;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class HandleCreatePlaylist : ICreateAggregateCommandHandler<PlaylistIdentity, Playlist, CreatePlaylistCommand, PlaylistCommandRejectionReason>
    {
        public CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandRejectionReason> Handle(CreatePlaylistCommand command)
        {
            var playlist = new Playlist();
            var events = playlist.Create(command).ToList();
            return new CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandRejectionReason>(playlist, events);
        }
    }
}
