using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Commands.Playlist
{
    public class PublishPlaylistCommand : CommandBase<PlaylistIdentity>
    {
        public PublishPlaylistCommand()
        {

        }

        public PublishPlaylistCommand(PlaylistIdentity targetIdentity, Guid id, string causedById, string correlationId) 
            : base(targetIdentity, id, causedById, correlationId)
        {
        }
    }
}
