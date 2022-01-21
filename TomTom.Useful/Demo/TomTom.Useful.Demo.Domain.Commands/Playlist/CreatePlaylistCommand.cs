using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Commands.Playlist
{
    public class CreatePlaylistCommand : CreateCommandBase<PlaylistIdentity>
    {
        public CreatePlaylistCommand(
            Guid id, 
            ExplorerIdentity explorerId,
            DateTime creationTimestamp,
            string title,
            string causedById ,
            string correlationId
            ) : base(id, causedById, correlationId)
        {
            ExplorerId = explorerId;
            CreationTimestamp = creationTimestamp;
            Title = title;
        }

        public ExplorerIdentity ExplorerId { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public string Title { get; set; }
    }

    public class AddSongToPlaylistCommand : CommandBase<PlaylistIdentity>
    {

    }
}
