using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Mongo
{
    internal sealed class DynamicMongoRepository<TIdentity, TEntity> : IKeyValueRepository<TIdentity,TEntity>, IPurger<TEntity>
        where TEntity: class
    {
        private readonly MongoRepository<TIdentity, DynamicMongoEntity<TIdentity, TEntity>> repository;
        private readonly Func<TEntity, TIdentity> identityExtractor;

        public DynamicMongoRepository(
            MongoRepository<TIdentity,DynamicMongoEntity<TIdentity, TEntity>> repository,
            Func<TEntity, TIdentity> identityExtractor)
        {
            this.repository = repository;
            this.identityExtractor = identityExtractor;
        }
        public Task<Result<object>> Delete(TIdentity identity)
        {
            return this.repository.Delete(identity);
        }

        public async Task<TEntity> Get(TIdentity identity)
        {
            var entity = await this.repository.Get(identity);

            return entity?.Data;
        }

        public Task<Result<object>> Insert(TEntity entity)
        {
            var mongoEntity = new DynamicMongoEntity<TIdentity, TEntity>(entity, this.identityExtractor(entity));

            return this.repository.Insert(mongoEntity);
        }

        public Task<Result<object>> Purge()
        {
            return this.repository.Purge();
        }

        public Task<Result<object>> Update(TEntity entity)
        {
            var mongoEntity = new DynamicMongoEntity<TIdentity,TEntity>(entity, this.identityExtractor(entity));

            return this.repository.Update(mongoEntity);
        }
    }

    internal class DynamicMongoEntity<TIdentity,TEntity> : MongoEntity<TIdentity>
    {
        public DynamicMongoEntity(TEntity data, TIdentity identity)
        {
            this.Data = data;
            this.BsonId = identity;
        }

        public DynamicMongoEntity()
        {
        }

        public TEntity Data { get; set; }
    }
}
