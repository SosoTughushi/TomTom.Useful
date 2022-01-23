using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing.CommandHandling;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class HandlePublishPlaylist : IAggregateCommandHandler<PlaylistIdentity, PublishPlaylistCommand, Playlist, PlaylistCommandRejectionReason>
    {
        public async Task<AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandRejectionReason>> Handle(PublishPlaylistCommand command, Playlist aggregate)
        {
            if(aggregate.Published)
            {
                return new AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandRejectionReason>(PlaylistCommandRejectionReason.PlaylistIsAlreadyPublished);
            }

            var events = aggregate.Publish(command).ToList();

            return new AggregateCommandHandlerResult<PlaylistIdentity, PlaylistCommandRejectionReason>(events);
        }
    }
}
