using Microsoft.Extensions.Hosting;
using TomTom.Useful.AsyncToSync;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Demo.Domain.Playlist.CommandHandlers;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.Demo.Application
{
    public class PlaylistWriter : IPlaylistWriter, IHostedService
    {
        private readonly AsyncToSyncConverter<Guid, ResultOfPlaylistCommand> asyncToSync =
            new AsyncToSyncConverter<Guid, ResultOfPlaylistCommand>(500, resultMessage => resultMessage.MessageId);

        private readonly ICommandPublisher<ICommand<PlaylistIdentity>> publisher;
        private readonly ISubscriber<ResultOfPlaylistCommand> commandResultsSubscriber;
        private IAsyncDisposable resultSubscriptionDisposer;

        public PlaylistWriter(ICommandPublisher<ICommand<PlaylistIdentity>> publisher, ISubscriber<ResultOfPlaylistCommand> commandResultsSubscriber)
        {
            this.publisher = publisher;
            this.commandResultsSubscriber = commandResultsSubscriber;
        }

        public async Task<CreatePlaylistResult> Create(string title, DemoAppContext context)
        {
            var command = PlaylistCommandFactory.Create(
                explorerId: context.CurrentUserId,
                title: title,
                causedById: context.RequestId,
                correlationId: context.CorrelationId
                );

            var result = await PublishAndGetResult(command);

            if (result.Success)
            {
                return new CreatePlaylistResult(result.Value.ToString());
            }

            return result.Error.Match(rejectionReason =>
            {
                switch (rejectionReason)
                {
                    case PlaylistCommandRejectionReason.PlaylistWithSameTitleAlreadyExists:
                        return new CreatePlaylistResult(CreatePlaylistFailureReason.TitleAlreadyExists);
                }

                return new CreatePlaylistResult(CreatePlaylistFailureReason.Unknown);
            },
            ex => throw ex);
        }

        public async Task<PublishPlaylistResult> Publish(Guid playlistId, DemoAppContext context)
        {
            var command = PlaylistCommandFactory.PublishPlaylist(
                playlistId: playlistId,
                causedById: context.RequestId,
                correlationId: context.CorrelationId);


            var result = await PublishAndGetResult(command);


            if (result.Success)
            {
                return new PublishPlaylistResult();
            }


            return result.Error.Match(rejected =>
            {
                switch (rejected)
                {
                    case PlaylistCommandRejectionReason.PlaylistIsAlreadyPublished:
                        return new PublishPlaylistResult(PublishPlaylistFailureReason.AlreadyPublished);
                }
                return new PublishPlaylistResult(PublishPlaylistFailureReason.Unknown);
            }, ex => throw ex);
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.resultSubscriptionDisposer = await this.commandResultsSubscriber.Subscribe(result =>
            {
                this.asyncToSync.SetResult(result);
                return Task.CompletedTask;
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (resultSubscriptionDisposer != null)
            {
                await resultSubscriptionDisposer.DisposeAsync();
            }
        }

        private async Task<ResultOfPlaylistCommand> PublishAndGetResult(ICommand<PlaylistIdentity> command)
        {
            var commandResultSubscriptionTask = _subscribeToCommandResult(command.MessageId);

            await publisher.Publish(command);

            var commandResult = await commandResultSubscriptionTask;

            return commandResult;
        }

        private async Task<ResultOfPlaylistCommand> _subscribeToCommandResult(Guid commandId)
        {
            return await this.asyncToSync.AwaitResult(commandId);
        }

    }


}
