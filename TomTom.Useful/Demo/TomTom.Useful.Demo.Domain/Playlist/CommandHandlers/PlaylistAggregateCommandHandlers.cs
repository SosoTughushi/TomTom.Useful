using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class PlaylistAggregateCommandHandlers : AggregateCommandHandlers<Playlist, PlaylistIdentity, PlaylistCommandValidationError>
    {
        public PlaylistAggregateCommandHandlers( 
            ISubscriber<ICommand<PlaylistIdentity>> subscriber
            , IEntityByKeyProvider<PlaylistIdentity, Playlist?> aggregateRepository
            , IEventPublisher publisher) 
            : base(subscriber, aggregateRepository, publisher)
        {
        }

        protected override void RegisterCommandHandlers()
        {
            var handleCreatePlaylist = new HandleCreatePlaylist();
            this.RegisterCreateCommandHandler<CreatePlaylistCommand>(handleCreatePlaylist.Handle);

            var handlePublishPlaylist = new HandlePublishPlaylist();
            this.RegisterCommandHandler<PublishPlaylistCommand>(handlePublishPlaylist.Handle);
        }
    }
}
