using TomTom.Useful.CQRS;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing.CommandHandling;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class PlaylistAggregateCommandHandlers : AggregateCommandHandlers<Playlist, PlaylistIdentity, PlaylistCommandRejectionReason>
    {
        private readonly IPublisher<ResultOfPlaylistCommand> resultPublisher;
        private readonly IFilteredListProvider<Playlist> filteredPlaylistProvider;

        public PlaylistAggregateCommandHandlers(
            IPublisher<ResultOfPlaylistCommand> resultPublisher
            , ISubscriber<ICommand<PlaylistIdentity>> subscriber
            , IEntityByKeyProvider<PlaylistIdentity, Playlist?> aggregateRepository
            , IFilteredListProvider<Playlist> filteredPlaylistProvider
            , IEventPublisher<PlaylistIdentity> publisher)
            : base(subscriber, aggregateRepository, publisher)
        {
            this.resultPublisher = resultPublisher;
            this.filteredPlaylistProvider = filteredPlaylistProvider;
        }

        protected override void RegisterCommandHandlers()
        {
            var handleCreatePlaylist = new HandleCreatePlaylist(filteredPlaylistProvider);
            this.RegisterCreateCommandHandler<CreatePlaylistCommand>(handleCreatePlaylist.Handle);

            var handlePublishPlaylist = new HandlePublishPlaylist();
            this.RegisterCommandHandler<PublishPlaylistCommand>(handlePublishPlaylist.Handle);
        }

        protected override async Task OnCommandSucceeded(ICommand<PlaylistIdentity> command, Playlist aggregate)
        {
            await this.resultPublisher.Publish(new ResultOfPlaylistCommand(aggregate.Id,command));
        }

        protected override async Task OnCommandRejected(PlaylistCommandRejectionReason error, ICommand<PlaylistIdentity> command, Playlist? aggregate = null)
        {
            await this.resultPublisher.Publish(new ResultOfPlaylistCommand(error, command));
        }

        protected override async Task OnException(ICommand<PlaylistIdentity> command, Exception ex)
        {
            await this.resultPublisher.Publish(new ResultOfPlaylistCommand(ex, command));
        }
    }

    public class ResultOfPlaylistCommand : Result<PlaylistIdentity, Either<PlaylistCommandRejectionReason, Exception>>, IMessage
    {
        public ResultOfPlaylistCommand(PlaylistIdentity playlistId,ICommand<PlaylistIdentity> playlistCommand) : base(playlistId)
        {
            this.MessageId = playlistCommand.MessageId;
            this.CausedById = playlistCommand.MessageId.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }

        public ResultOfPlaylistCommand(PlaylistCommandRejectionReason rejectionReason,ICommand<PlaylistIdentity> playlistCommand) 
            : base(new Either<PlaylistCommandRejectionReason, Exception>(rejectionReason))
        {
            this.MessageId = playlistCommand.MessageId;
            this.CausedById = playlistCommand.MessageId.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }


        public ResultOfPlaylistCommand(Exception exception, ICommand<PlaylistIdentity> playlistCommand)
            : base(new Either<PlaylistCommandRejectionReason, Exception>(exception))
        {
            this.MessageId = playlistCommand.MessageId;
            this.CausedById = playlistCommand.MessageId.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }

        public Guid MessageId { get; set; }

        public string CorrelationId { get; }

        public string CausedById { get; }

        public ICommand<PlaylistIdentity> PlaylistCommand { get; }
    }
}
