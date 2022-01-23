using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.Projections
{
    public class ProjectedPlaylistInMemoryRepository :
        IWriter<Guid, ProjectedPlaylist>
        , IEntityByKeyProvider<Guid, ProjectedPlaylist?>
        , IEntityByKeyProvider<Guid, List<ProjectedPlaylist>>
    {

        private readonly ConcurrentDictionary<Guid, ProjectedPlaylist> PlaylistsById =
            new ConcurrentDictionary<Guid, ProjectedPlaylist>();

        private readonly ConcurrentDictionary<Guid, Dictionary<Guid, ProjectedPlaylist>> PlaylistsByUsers =
            new ConcurrentDictionary<Guid, Dictionary<Guid, ProjectedPlaylist>>();


        public Task<Result<object>> Insert(ProjectedPlaylist entity)
        {
            PlaylistsById.TryAdd(entity.Id, entity);

            PlaylistsByUsers.AddOrUpdate(entity.OwnerId,
                _ => new Dictionary<Guid, ProjectedPlaylist> { { entity.OwnerId, entity } }
                , (_, dictionary) =>
                 {
                     dictionary.Add(entity.OwnerId, entity);
                     return dictionary;
                 });


            return Ok();
        }
        public Task<Result<object>> Update(ProjectedPlaylist entity)
        {
            PlaylistsById.AddOrUpdate(entity.Id, entity, (key, existing) => entity);

            PlaylistsByUsers.AddOrUpdate(entity.OwnerId,
                key => new Dictionary<Guid, ProjectedPlaylist> { { entity.OwnerId, entity } }
                , (key, dictionary) =>
                {
                    dictionary[entity.OwnerId] = entity;
                    return dictionary;
                });

            return Ok();
        }

        public Task<Result<object>> Delete(Guid identity)
        {
            if (PlaylistsById.TryRemove(identity, out var entity))
            {
                PlaylistsByUsers.AddOrUpdate(entity.OwnerId,
                    _ => new Dictionary<Guid, ProjectedPlaylist> { { entity.OwnerId, entity } },
                    (_, dictionary) =>
                    {
                        dictionary.Remove(entity.Id);
                        return dictionary;
                    });
            }
            return Ok();
        }

        public Task<ProjectedPlaylist?> Get(Guid identity)
        {
            if (PlaylistsById.TryGetValue(identity, out var entity))
            {
                return Task.FromResult<ProjectedPlaylist?>(entity);
            }

            return Task.FromResult<ProjectedPlaylist?>(default); ;
        }

        Task<List<ProjectedPlaylist>> IEntityByKeyProvider<Guid, List<ProjectedPlaylist>>.Get(Guid userId)
        {
            if(PlaylistsByUsers.TryGetValue(userId, out var values))
            {
                var result = values.Select(c=>c.Value).ToList();
                return Task.FromResult(result);
            }

            return Task.FromResult(new List<ProjectedPlaylist>());
        }


        private static Task<Result<object>> Ok() => Task.FromResult(Result.Ok<object>());
    }
}
