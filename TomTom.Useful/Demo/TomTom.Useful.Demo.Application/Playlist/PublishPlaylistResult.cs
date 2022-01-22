using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Demo.Application
{
    public class PublishPlaylistResult : Result<PublishPlaylistFailureReason>
    {
        public PublishPlaylistResult()
        {
        }

        public PublishPlaylistResult(PublishPlaylistFailureReason error) : base(error)
        {
        }
    }

    public enum PublishPlaylistFailureReason
    {
        AlreadyPublished,
        Unknown
    }
}
