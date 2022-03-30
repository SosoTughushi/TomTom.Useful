using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Demo.Application.Commands.Playlist
{
    public class CreatePlaylistResult : Result<Guid, CreatePlaylistFailureReason>
    {
        public CreatePlaylistResult(Guid newId) : base(newId)
        {
        }

        public CreatePlaylistResult(CreatePlaylistFailureReason error) : base(error)
        {
        }
    }

    public enum CreatePlaylistFailureReason
    {
        Unknown,
        TitleAlreadyExists
    }
}
