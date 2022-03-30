using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Repositories.Abstractions;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using TomTom.Useful.EventSourcing;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.Demo.Domain.Dal
{
    public class PlaylistInMemoryRepository :
        IEntityByKeyProvider<PlaylistIdentity, Playlist.Playlist?>
        , IFilteredListProvider<Playlist.Playlist>
        , IHostedService
    {
        private readonly ConcurrentDictionary<PlaylistIdentity, Playlist.Playlist> playlists = new();
        private readonly ISubscriber<Event<PlaylistIdentity>> eventSubscriber;
        private IAsyncDisposable? subscription;

        public PlaylistInMemoryRepository(ISubscriber<Event<PlaylistIdentity>> eventSubscriber)
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

        public Task<IEnumerable<Playlist.Playlist>> GetFiltered(System.Linq.Expressions.Expression<Func<Playlist.Playlist, bool>> filterExpression)
        {
            var compiledExpression = filterExpression.Compile();
            var playlistsFiltered = this.playlists.Values.Where(compiledExpression); // unsafe

            return Task.FromResult(playlistsFiltered);
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

        public void OnEvent(Event<PlaylistIdentity> @event)
        {
            var playlist = playlists.GetOrAdd(@event.SourceAggregateId, (key) => new Playlist.Playlist());

            AggregateEventApplier<PlaylistIdentity, Playlist.Playlist>.ApplyEvents(playlist, Enumerable.Repeat(@event, 1));
        }
    }
}