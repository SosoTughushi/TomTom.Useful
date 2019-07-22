using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Mongo
{
    public class MongoRepository<TIdentity, TMongoEntity> 
        : IKeyValueRepository<TIdentity, TMongoEntity>
        , IPurger<TMongoEntity>
        where TMongoEntity : MongoEntity<TIdentity>
    {
        private static readonly FindOneAndReplaceOptions<TMongoEntity> UpdateOptions
            = new FindOneAndReplaceOptions<TMongoEntity> { IsUpsert = true };

        protected readonly IMongoDatabase database;
        protected readonly IMongoCollection<TMongoEntity> collection;
        protected readonly ResultFactory<object> resultFactory = Result.GetFactory<object>();

        public MongoRepository(MongoConifgurations conifgurations)
        {
            // TODO: Add guard clauses for mongoUrl and schema (not null or empty)
            var mongoUrlObject = new MongoUrl(conifgurations.MongoUrl);
            var client = new MongoClient(mongoUrlObject);
            this.database = client.GetDatabase(mongoUrlObject.DatabaseName);
            this.collection = this.database.GetCollection<TMongoEntity>(string.Join(".", conifgurations.Schema, typeof(TMongoEntity).Name));
        }

        public async Task<TMongoEntity> Get(TIdentity identity)
        {
            var filter = Builders<TMongoEntity>.Filter.Eq(x => x.BsonId, identity);
            var document = await this.collection.Find(filter).FirstOrDefaultAsync();
            return document;
        }

        public async Task<Result<object>> Insert(TMongoEntity entity)
        {
            var filter = Builders<TMongoEntity>.Filter.Eq(x => x.BsonId, entity.BsonId);

            await this.collection.FindOneAndReplaceAsync(filter, entity, UpdateOptions);

            // todo: return proper result
            return resultFactory.Ok();
        }

        public Task<Result<object>> Update(TMongoEntity entity)
        {
            return this.Insert(entity);
        }

        public async Task<Result<object>> Delete(TIdentity identity)
        {
            var filter = Builders<TMongoEntity>.Filter.Eq(x => x.BsonId, identity);
            var document = await this.collection.FindOneAndDeleteAsync(filter);

            if (document != null)
            {
                return resultFactory.Ok();
            }

            return resultFactory.Fail("does not exist"); // todo: come up with proper errors
        }

        public async Task<Result<object>> Purge()
        {
            var name = this.collection.CollectionNamespace.CollectionName;

            await this.database.DropCollectionAsync(name);
            await this.database.CreateCollectionAsync(name);

            return resultFactory.Ok();
        }
    }
}
