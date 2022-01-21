using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class HandlePublishPlaylist : IAggregateCommandHandler<PlaylistIdentity, PublishPlaylistCommand, Playlist, PlaylistCommandValidationError>
    {
        public AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandValidationError> Handle(PublishPlaylistCommand command, Playlist aggregate)
        {
            if(aggregate.Published)
            {
                return new AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandValidationError>(PlaylistCommandValidationError.AlreadyPublished);
            }

            var events = aggregate.Publish(command);

            return new AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandValidationError>(events);
        }
    }
}
