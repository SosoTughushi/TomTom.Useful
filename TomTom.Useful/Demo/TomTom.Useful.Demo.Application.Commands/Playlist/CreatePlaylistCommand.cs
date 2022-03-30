namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public class CreatePlaylistCommand : CommandBase<CreatePlaylistResult>
    {
        public CreatePlaylistCommand(
            Guid messageId, 
            Guid explorerId,
            DateTime creationTimestamp,
            string title,
            string causedById ,
            string correlationId
            ) : base(messageId, causedById, correlationId)
        {
            ExplorerId = explorerId;
            CreationTimestamp = creationTimestamp;
            Title = title;
        }

        public Guid ExplorerId { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public string Title { get; set; }
    }
}
