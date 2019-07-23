using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Mongo
{
    public class MongoRepository<TIdentity, TMongoEntity> 
        : IKeyValueRepository<TIdentity, TMongoEntity>,
        IPurger<TMongoEntity>,
        IListProvider<TMongoEntity>,
        IPagedListProvider<TMongoEntity>,
        IFilteredListProvider<TMongoEntity>,
        IPagedFilteredListProvider<TMongoEntity>,
        ISortedListProvider<TMongoEntity>,
        IPagedSortedListProvider<TMongoEntity>,
        IFilteredSortedListProvider<TMongoEntity>,
        IPagedFilteredSortedListProvider<TMongoEntity>
        where TMongoEntity : MongoEntity<TIdentity>
    {
        private static readonly FindOneAndReplaceOptions<TMongoEntity> UpdateOptions
            = new FindOneAndReplaceOptions<TMongoEntity> { IsUpsert = true };

        protected readonly IMongoDatabase database;
        protected readonly IMongoCollection<TMongoEntity> collection;
        protected readonly ResultFactory<object> resultFactory = Result.GetFactory<object>();

        public MongoRepository(MongoConifgurations conifgurations, IMongoDatabase database)
        {
            // TODO: Add guard clauses for mongoUrl and schema (not null or empty)
            this.database = database;
            this.collection = this.database.GetCollection<TMongoEntity>(string.Join(".", conifgurations.Schema, typeof(TMongoEntity).Name));
        }

        public async Task<TMongoEntity> Get(TIdentity identity)
        {
            var filter = Builders<TMongoEntity>.Filter.Eq(x => x.BsonId, identity);
            var document = await this.collection.Find(filter).FirstOrDefaultAsync();
            return document;
        }

        public async Task<IEnumerable<TMongoEntity>> GetAll()
        {
            var entities = await this.collection.Find(c => true).ToListAsync();

            return entities;
        }

        public async Task<IEnumerable<TMongoEntity>> GetFiltered(Expression<Func<TMongoEntity, bool>> filterExpression)
        {
            var entities = await this.collection.Find(filterExpression).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<TMongoEntity>> GetSorted(Expression<Func<TMongoEntity, object>> sortExpression)
        {
            var entities = await this.collection.Find(c => true).SortBy(sortExpression).ToListAsync();
            return entities;
        }


        public async Task<IEnumerable<TMongoEntity>> GetSortedDesc(Expression<Func<TMongoEntity, object>> sortExpression)
        {
            var entities = await this.collection.Find(c => true).SortByDescending(sortExpression).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<TMongoEntity>> GetFilteredSorted(
            Expression<Func<TMongoEntity, bool>> filterExpression,
            Expression<Func<TMongoEntity, object>> sortExpression)
        {
            var entities = await this.collection.Find(filterExpression).SortBy(sortExpression).ToListAsync();

            return entities;
        }


        public async Task<IEnumerable<TMongoEntity>> GetFilteredSortedDesc(Expression<Func<TMongoEntity, bool>> filterExpression, Expression<Func<TMongoEntity, object>> sortExpression)
        {
            var entities = await this.collection.Find(filterExpression).SortByDescending(sortExpression).ToListAsync();

            return entities;
        }

        public Task<PagedResult<TMongoEntity>> GetPaged(int skip, int take)
        {
            var collection = this.collection.Find(c => true);
            return GetPaged(collection, skip, take);
        }

        public Task<PagedResult<TMongoEntity>> GetPagedFiltered(Expression<Func<TMongoEntity, bool>> filterExpression, int skip, int take)
        {
            var collection = this.collection.Find(filterExpression);
            return GetPaged(collection, skip, take);
        }

        public Task<PagedResult<TMongoEntity>> GetPagedSorted(Expression<Func<TMongoEntity, object>> sortExpression, int skip, int take)
        {
            var collection = this.collection.Find(c => true).SortBy(sortExpression);
            return GetPaged(collection, skip, take);
        }

        public Task<PagedResult<TMongoEntity>> GetPagedSortedDesc(Expression<Func<TMongoEntity, object>> sortExpression, int skip, int take)
        {
            var collection = this.collection.Find(c => true).SortByDescending(sortExpression);
            return GetPaged(collection, skip, take);
        }

        public Task<PagedResult<TMongoEntity>> GetPagedFilteredSorted(Expression<Func<TMongoEntity, bool>> filterExpression, Expression<Func<TMongoEntity, object>> sortExpression, int skip, int take)
        {
            var collection = this.collection.Find(filterExpression).SortBy(sortExpression);
            return GetPaged(collection, skip, take);
        }


        public Task<PagedResult<TMongoEntity>> GetPagedFilteredSortedDesc(Expression<Func<TMongoEntity, bool>> filterExpression, Expression<Func<TMongoEntity, object>> sortExpression, int skip, int take)
        {
            var collection = this.collection.Find(filterExpression).SortByDescending(sortExpression);
            return GetPaged(collection, skip, take);
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

        private static async Task<PagedResult<TMongoEntity>> GetPaged(IFindFluent<TMongoEntity, TMongoEntity> collection, int skip, int take)
        {
            var total = await collection.CountDocumentsAsync();
            var entities = await collection.Skip(skip).Limit(take).ToListAsync();

            return new PagedResult<TMongoEntity>(entities, skip, (int)total);
        }
    }
}
