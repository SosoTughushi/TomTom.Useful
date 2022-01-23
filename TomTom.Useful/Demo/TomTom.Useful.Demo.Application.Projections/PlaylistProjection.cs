using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.Projections
{
    public class PlaylistProjection : IHostedService
    {
        private readonly ISubscriber<Event<PlaylistIdentity>> subscriber;
        private readonly IWriter<Guid, ProjectedPlaylist> projectedPlaylistWriter;
        private readonly IEntityByKeyProvider<Guid, ProjectedPlaylist> projectedPlaylistByKeyProvider;
        private IAsyncDisposable? subscriptionDisposer;

        public PlaylistProjection(
            ISubscriber<Event<PlaylistIdentity>> subscriber
            , IWriter<Guid, ProjectedPlaylist> projectedPlaylistWriter
            , IEntityByKeyProvider<Guid,ProjectedPlaylist> projectedPlaylistByKeyProvider
            )
        {
            this.subscriber = subscriber;
            this.projectedPlaylistWriter = projectedPlaylistWriter;
            this.projectedPlaylistByKeyProvider = projectedPlaylistByKeyProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.subscriptionDisposer = await this.subscriber.Subscribe(HandleEvent);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.subscriptionDisposer != null)
            {
                await this.subscriptionDisposer.DisposeAsync();
            }
        }

        private async Task<Result> HandleEvent(Event<PlaylistIdentity> playlistEvent, ICurrentMessageContext context)
        {
            if (playlistEvent is PlaylistCreatedEvent playlistCreated)
            {
                return await this.HandlePlaylistCreated(playlistCreated);
            }

            if(playlistEvent is PlaylistPublished playlistPublished)
            {
                return await this.HandlePlaylistPublished(playlistPublished);
            }

            return Result.Ok();
        }

        private async Task<Result> HandlePlaylistCreated(PlaylistCreatedEvent playlistCreated)
        {
            var projectedPlaylist = new ProjectedPlaylist(
                Id: playlistCreated.SourceAggregateId,
                Title: playlistCreated.Title,
                IsPublished: false,
                OwnerId: playlistCreated.ExplorerId,
                Version: playlistCreated.SourceAggregateVersion);

            var result = await this.projectedPlaylistWriter.Insert(projectedPlaylist);

            return result;
        }

        private async Task<Result> HandlePlaylistPublished(PlaylistPublished playlistPublished)
        {
            var playlist = await this.projectedPlaylistByKeyProvider.Get(playlistPublished.SourceAggregateId);

            var publishedPlaylist = playlist with { IsPublished = true };

            var result = await this.projectedPlaylistWriter.Update(publishedPlaylist);

            return result;
        }
    }
}
