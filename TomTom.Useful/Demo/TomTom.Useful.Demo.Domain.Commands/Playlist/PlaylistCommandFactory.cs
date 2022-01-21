using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.Domain.Commands.Playlist
{
    public static class PlaylistCommandFactory
    {
        public static CreatePlaylistCommand Create(
            ExplorerIdentity explorerId
            , string title
            , string causedById
            , string correlationId)
        {
            return new CreatePlaylistCommand(
                id: Guid.NewGuid(),
                explorerId,
                creationTimestamp: DateTime.UtcNow,
                title: title,
                causedById: causedById,
                correlationId: correlationId);
        }

        public static PublishPlaylistCommand PublishPlaylist(
            PlaylistIdentity playlistId
            , string causedById
            , string correlationId
            )
        {
            return new PublishPlaylistCommand(
                playlistId,
                Guid.NewGuid(),
                causedById,
                correlationId
                );
        }
    }


}
