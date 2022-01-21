using TomTom.Useful.Demo.Domain.Commands.Playlist;
using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Playlist
{
    public partial class Playlist : IAggregate<PlaylistIdentity>
        , IEmitsEvent<PlaylistCreatedEvent>
        , IEmitsEvent<PlaylistPublished>
    {
        public Playlist()
        {
        }

        public PlaylistIdentity Id { get; private set; }

        public long Version { get; set; }
        public string Title { get; private set; }
        public DateTime CreationTimestamp { get; private set; }
        public ExplorerIdentity ExplorerId { get; private set; }
        public bool Published { get; private set; }

        public IEnumerable<Event<PlaylistIdentity>> Create(CreatePlaylistCommand command)
        {
            yield return this.EmitEvent(PlaylistEventFactory.PlaylistCreated(command.Id, command.CorrelationId, command.Title, command.ExplorerId, command.CreationTimestamp));
        }

        public IEnumerable<Event<PlaylistIdentity>> Publish(PublishPlaylistCommand command)
        {
            yield return this.EmitEvent(PlaylistEventFactory.PlaylistPublished(this.Id, this.Version, command.Id, command.CorrelationId));
        }

        public void Apply(PlaylistCreatedEvent @event)
        {
            this.Id = @event.SourceAggregateId;
            this.Title = @event.Title;
            this.CreationTimestamp = @event.CreationTimestamp;
            this.ExplorerId = @event.ExplorerId;
        }

        public void Apply(PlaylistPublished @event)
        {
            this.Published = true;
        }
    }
}