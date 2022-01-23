using TomTom.Useful.Demo.Application.Projections;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.Playlist
{
    public class PlaylistReader : IPlaylistReader
    {
        private readonly IEntityByKeyProvider<Guid, ProjectedPlaylist?> playlistByIdProvider;
        private readonly IEntityByKeyProvider<Guid, List<ProjectedPlaylist>> playlistsByOwnerProvider;

        public PlaylistReader(
            IEntityByKeyProvider<Guid, ProjectedPlaylist?> playlistByIdProvider,
            IEntityByKeyProvider<Guid, List<ProjectedPlaylist>> playlistsByUserProvider)
        {
            this.playlistByIdProvider = playlistByIdProvider;
            this.playlistsByOwnerProvider = playlistsByUserProvider;
        }

        public async Task<PlaylistDto?> Get(Guid playlistId, DemoRequestContext context)
        {
            var playlist = await playlistByIdProvider.Get(playlistId);

            if (playlist == null)
            {
                return null;
            }

            if (playlist.OwnerId != context.CurrentUserId)
            {
                return null;
            }
            return MapToDto(playlist);
        }

        public async Task<List<PlaylistDto>> GetAll(DemoRequestContext context)
        {
            var playlists = await this.playlistsByOwnerProvider.Get(context.CurrentUserId);

            return playlists.Select(MapToDto).ToList();
        }

        private static PlaylistDto MapToDto(ProjectedPlaylist playlist)
        {
            return new PlaylistDto(playlist.Id, playlist.Title, playlist.IsPublished);
        }
    }
}
