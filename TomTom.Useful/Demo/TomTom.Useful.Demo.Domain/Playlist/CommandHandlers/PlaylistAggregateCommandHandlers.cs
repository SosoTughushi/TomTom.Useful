using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain.Playlist.CommandHandlers
{
    public class PlaylistAggregateCommandHandlers : AggregateCommandHandlers<Playlist, PlaylistIdentity, PlaylistCommandRejectionReason>
    {
        private readonly IPublisher<ResultOfPlaylistCommand> resultPublisher;

        public PlaylistAggregateCommandHandlers(
            IPublisher<ResultOfPlaylistCommand> resultPublisher
            , ISubscriber<ICommand<PlaylistIdentity>> subscriber
            , IEntityByKeyProvider<PlaylistIdentity, Playlist?> aggregateRepository
            , IEventPublisher publisher)
            : base(subscriber, aggregateRepository, publisher)
        {
            this.resultPublisher = resultPublisher;
        }

        protected override void RegisterCommandHandlers()
        {
            var handleCreatePlaylist = new HandleCreatePlaylist();
            this.RegisterCreateCommandHandler<CreatePlaylistCommand>(handleCreatePlaylist.Handle);

            var handlePublishPlaylist = new HandlePublishPlaylist();
            this.RegisterCommandHandler<PublishPlaylistCommand>(handlePublishPlaylist.Handle);
        }

        protected override async Task OnCommandSucceeded(ICommand<PlaylistIdentity> command, Playlist aggregate)
        {
            await this.resultPublisher.Publish(new ResultOfPlaylistCommand(command));
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

    public class ResultOfPlaylistCommand : Result<Either<PlaylistCommandRejectionReason, Exception>>, IMessage
    {
        public ResultOfPlaylistCommand(ICommand<PlaylistIdentity> playlistCommand) : base()
        {
            this.Id = playlistCommand.Id;
            this.CausedById = playlistCommand.Id.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }

        public ResultOfPlaylistCommand(PlaylistCommandRejectionReason rejectionReason,ICommand<PlaylistIdentity> playlistCommand) 
            : base(new Either<PlaylistCommandRejectionReason, Exception>(rejectionReason))
        {
            this.Id = playlistCommand.Id;
            this.CausedById = playlistCommand.Id.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }


        public ResultOfPlaylistCommand(Exception exception, ICommand<PlaylistIdentity> playlistCommand)
            : base(new Either<PlaylistCommandRejectionReason, Exception>(exception))
        {
            this.Id = playlistCommand.Id;
            this.CausedById = playlistCommand.Id.ToString();
            this.CorrelationId = playlistCommand.CorrelationId;
            this.PlaylistCommand = playlistCommand;
        }

        public Guid Id { get; set; }

        public string CorrelationId { get; }

        public string CausedById { get; }

        public ICommand<PlaylistIdentity> PlaylistCommand { get; }
    }


    public class Either<T1, T2>
    {
        private bool isLeft;
        public T1 Left { get; }

        public T2 Right { get; }

        public Either(T1 left)
        {
            this.Left = left;
            this.isLeft = true;
        }

        public Either(T2 right)
        {
            this.Right = right;
            this.isLeft = false;
        }

        public TResult Match<TResult>(Func<T1, TResult> leftMatch, Func<T2, TResult> rightMatch)
        {
            if (isLeft)
            {
                return leftMatch(Left);
            }

            return rightMatch(Right);
        }
    }
}
