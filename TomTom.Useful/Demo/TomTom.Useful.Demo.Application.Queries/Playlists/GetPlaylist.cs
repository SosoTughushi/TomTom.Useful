using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Application.Queries.Playlists
{
    public class GetPlaylist : QueryBase<PlaylistDto>
    {
        public GetPlaylist(
            Guid playlistId,
            Guid messageId, 
            string correlationId, 
            string causedById) : base(messageId, correlationId, causedById)
        {
            PlaylistId = playlistId;
        }

        public Guid PlaylistId { get; }
    }
}
