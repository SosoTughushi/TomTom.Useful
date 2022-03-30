namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public class CreatePlaylistCommand : CommandBase<CreatePlaylistResult>
    {
        public CreatePlaylistCommand(
            string title,
            DemoRequestContext context
            ) : base(context)
        {
            ExplorerId = context.CurrentUserId;
            Title = title;
        }

        public Guid ExplorerId { get; set; }
        public string Title { get; set; }
    }
}
