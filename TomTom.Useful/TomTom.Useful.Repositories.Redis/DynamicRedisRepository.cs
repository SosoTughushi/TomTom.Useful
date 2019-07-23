using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Redis
{
    public class DynamicRedisRepository<TIdentity, TEntity> :
        IKeyValueRepository<TIdentity, TEntity>,
        IPurger<TEntity>,
        IListProvider<TEntity>
        where TEntity: class
    {
        private readonly RedisRepository<DynamicRedisEntity<TEntity>> redisRepository;
        private readonly Func<TEntity, TIdentity> identityExtractor;
        private readonly Func<TIdentity, string> identityToString;

        public DynamicRedisRepository(
            RedisRepository<DynamicRedisEntity<TEntity>> repository,
            Func<TEntity, TIdentity> identityExtractor,
            Func<TIdentity,string> identityToString)
        {
            this.redisRepository = repository;
            this.identityExtractor = identityExtractor;
            this.identityToString = identityToString;
        }

        public Task<Result<object>> Delete(TIdentity identity)
        {
            return this.redisRepository.Delete(identityToString(identity));
        }

        public async Task<TEntity> Get(TIdentity identity)
        {
            var entity = await this.redisRepository.Get(identityToString(identity));
            return entity?.Data;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var entities = await this.redisRepository.GetAll();

            return entities.Select(c => c.Data);
        }

        public Task<Result<object>> Insert(TEntity entity)
        {
            var dynamicEntity = CreateDynamicEntity(entity);

            return this.redisRepository.Insert(dynamicEntity);
        }

        public Task<Result<object>> Purge()
        {
            return this.redisRepository.Purge();
        }

        public Task<Result<object>> Update(TEntity entity)
        {
            var dynamicEntity = CreateDynamicEntity(entity);

            return this.redisRepository.Update(dynamicEntity);
        }

        private DynamicRedisEntity<TEntity> CreateDynamicEntity(TEntity entity)
        {
            var identity = identityExtractor(entity);
            var redisKey = identityToString(identity);

            var dynamicEntity = new DynamicRedisEntity<TEntity>(entity, redisKey);
            return dynamicEntity;
        }
    }

    public class DynamicRedisEntity<TData> : RedisEntity
    {
        public DynamicRedisEntity(TData data, string id)
        {
            Data = data;
            Id = id;
        }

        public TData Data { get; }
    }
}
