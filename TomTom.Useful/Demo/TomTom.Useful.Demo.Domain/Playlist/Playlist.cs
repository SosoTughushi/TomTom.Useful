using TomTom.Useful.Demo.Domain.Events.Playlist;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.EventSourcing;

namespace TomTom.Useful.Demo.Domain.Playlist
{
    public class Playlist : AggregateBase<PlaylistIdentity>
        , IEmitsEvent<PlaylistCreatedEvent>
        , IEmitsEvent<PlaylistPublished>
    {
        public Playlist()
        {
        }
        public Playlist(string title, ExplorerIdentity ownerId, DateTime creationTimestamp)
        {
            this.EmitEvent<Playlist, PlaylistIdentity, PlaylistCreatedEvent>(PlaylistEventFactory.PlaylistCreated(title, ownerId, creationTimestamp));
        }

        public PlaylistIdentity Id { get; private set; }

        public string Title { get; private set; }
        public DateTime CreationTimestamp { get; private set; }
        public ExplorerIdentity ExplorerId { get; private set; }
        public bool Published { get; private set; }

        public bool Publish()
        {
            if (this.Published)
            {
                return false;
            }

            this.EmitEvent<Playlist, PlaylistIdentity, PlaylistPublished>(PlaylistEventFactory.PlaylistPublished());

            return true;
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