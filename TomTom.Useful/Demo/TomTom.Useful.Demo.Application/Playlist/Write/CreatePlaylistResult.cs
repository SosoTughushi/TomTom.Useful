using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Demo.Application.Playlist
{
    public class CreatePlaylistResult : Result<string, CreatePlaylistFailureReason>
    {
        public CreatePlaylistResult(string value) : base(value)
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
