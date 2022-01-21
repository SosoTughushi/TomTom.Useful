using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class HandleCreatePlaylist : ICreateAggregateCommandHandler<PlaylistIdentity, Playlist, CreatePlaylistCommand, PlaylistCommandValidationError>
    {
        public CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandValidationError> Handle(CreatePlaylistCommand command)
        {
            var playlist = new Playlist();
            var events = playlist.Create(command).ToList();
            return new CreateAggregateCommandHandlerResult<PlaylistIdentity, Playlist, PlaylistCommandValidationError>(playlist, events);
        }
    }
}
