using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Repositories.Abstractions;
using TomTom.Useful.Demo.Domain.Playlist;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;
using TomTom.Useful.Demo.Domain.Events.Playlist;

namespace TomTom.Useful.Demo.Domain.Dal
{
    public class PlaylistInMemoryRepository : IEntityByKeyProvider<PlaylistIdentity, Playlist.Playlist?>, IHostedService
    {
        private readonly ConcurrentDictionary<PlaylistIdentity, Playlist.Playlist> playlists = new ConcurrentDictionary<PlaylistIdentity, Playlist.Playlist>();
        private readonly ISubscriber<Event> eventSubscriber;
        private IAsyncDisposable? subscription;

        public PlaylistInMemoryRepository(ISubscriber<Event> eventSubscriber)
        {
            this.eventSubscriber = eventSubscriber;
        }

        public Task<Playlist.Playlist?> Get(PlaylistIdentity identity)
        {
            if (playlists.TryGetValue(identity, out var playlist))
            {
                return Task.FromResult<Playlist.Playlist?>(playlist);
            }

            return Task.FromResult(default(Playlist.Playlist));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.subscription = await this.eventSubscriber.Subscribe(ev =>
            {
                OnEvent(ev);
                return Task.CompletedTask;
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (subscription != null)
            {
                await subscription.DisposeAsync();
            }
        }

        public void OnEvent(Event @event)
        {
            if (@event is not PlaylistEventBase playlistEvent)
            {
                return;
            }

            var playlist = playlists.GetOrAdd(playlistEvent.SourceAggregateId, (key) => new Playlist.Playlist());


            AggregateEventApplier<Playlist.Playlist>.ApplyEvents(playlist, Enumerable.Repeat(playlistEvent, 1));
        }
    }
}