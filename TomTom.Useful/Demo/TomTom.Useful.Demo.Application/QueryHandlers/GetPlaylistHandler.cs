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
    public class GetPlaylistHandler : IQueryHandler<GetPlaylist, PlaylistDto?>
    {
        private readonly IEntityByKeyProvider<Guid, ProjectedPlaylist?> playlistByIdProvider;

        public GetPlaylistHandler(IEntityByKeyProvider<Guid, ProjectedPlaylist?> playlistByIdProvider)
        {
            this.playlistByIdProvider = playlistByIdProvider;
        }

        public async Task<PlaylistDto?> Handle(GetPlaylist query, CancellationToken cancellationToken)
        {
            var playlist = await playlistByIdProvider.Get(query.PlaylistId);

            if (playlist == null)
            {
                return null;
            }

            if (playlist.OwnerId != query.Context.CurrentUserId)
            {
                return null;
            }

            return MapToDto(playlist);
        }

        private static PlaylistDto MapToDto(ProjectedPlaylist playlist)
        {
            return new PlaylistDto(playlist.Id, playlist.Title, playlist.IsPublished);
        }
    }
}
