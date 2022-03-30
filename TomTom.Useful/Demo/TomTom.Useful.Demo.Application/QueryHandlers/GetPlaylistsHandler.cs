using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Application.Projections;
using TomTom.Useful.Demo.Application.Queries.Playlists;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.QueryHandlers
{
    public class GetPlaylistsHandler : IQueryHandler<GetPlaylists, List<PlaylistDto>>
    {
        private readonly IEntityByKeyProvider<Guid, List<ProjectedPlaylist>> playlistsByOwnerProvider;

        public GetPlaylistsHandler(IEntityByKeyProvider<Guid, List<ProjectedPlaylist>> playlistsByOwnerProvider)
        {
            this.playlistsByOwnerProvider = playlistsByOwnerProvider;
        }

        public async Task<List<PlaylistDto>> Handle(GetPlaylists request, CancellationToken cancellationToken)
        {
            var playlists = await this.playlistsByOwnerProvider.Get(request.Context.CurrentUserId);

            return playlists.Select(MapToDto).ToList();

        }

        private static PlaylistDto MapToDto(ProjectedPlaylist playlist)
        {
            return new PlaylistDto(playlist.Id, playlist.Title, playlist.IsPublished);
        }
    }
}
