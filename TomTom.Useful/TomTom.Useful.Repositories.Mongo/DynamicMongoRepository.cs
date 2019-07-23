using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;
using TomTom.Useful.ExpressionTreeExtensions;

namespace TomTom.Useful.Repositories.Mongo
{
    internal sealed class DynamicMongoRepository<TIdentity, TEntity> :
        ICrud<TIdentity,TEntity>,
        IPurger<TEntity>,
        IListProvider<TEntity>,
        IPagedListProvider<TEntity>,
        IFilteredListProvider<TEntity>,
        IPagedFilteredListProvider<TEntity>,
        ISortedListProvider<TEntity>,
        IPagedSortedListProvider<TEntity>,
        IFilteredSortedListProvider<TEntity>,
        IPagedFilteredSortedListProvider<TEntity>
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

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var entities = await this.repository.GetAll();

            return entities.Select(c => c.Data);
        }

        public async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filterExpression)
        {
            var entities = await this.repository.GetFiltered(Extend(filterExpression));
            return entities.Select(c => c.Data);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredSorted(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>> sortExpression)
        {
            var entities = await this.repository.GetFilteredSorted(Extend(filterExpression), Extend(sortExpression));

            return entities.Select(c => c.Data);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredSortedDesc(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>> sortExpression)
        {
            var entities = await this.repository.GetFilteredSortedDesc(Extend(filterExpression), Extend(sortExpression));

            return entities.Select(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPaged(int skip, int take)
        {
            var result = await this.repository.GetPaged(skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPagedFiltered(Expression<Func<TEntity, bool>> filterExpression, int skip, int take)
        {
            var result = await this.repository.GetPagedFiltered(Extend(filterExpression), skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPagedFilteredSorted(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>> sortExpression, int skip, int take)
        {
            var result = await this.repository.GetPagedFilteredSorted(Extend(filterExpression), Extend(sortExpression), skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPagedFilteredSortedDesc(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>> sortExpression, int skip, int take)
        {
            var result = await this.repository.GetPagedFilteredSortedDesc(Extend(filterExpression), Extend(sortExpression), skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPagedSorted(Expression<Func<TEntity, object>> sortExpression, int skip, int take)
        {
            var result = await this.repository.GetPagedSorted(Extend(sortExpression), skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<PagedResult<TEntity>> GetPagedSortedDesc(Expression<Func<TEntity, object>> sortExpression, int skip, int take)
        {
            var result = await this.repository.GetPagedSortedDesc(Extend(sortExpression), skip, take);

            return result.Convert(c => c.Data);
        }

        public async Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, object>> sortExpression)
        {
            var sorted = await this.repository.GetSorted(Extend(sortExpression));

            return sorted.Select(c => c.Data);
        }


        public async Task<IEnumerable<TEntity>> GetSortedDesc(Expression<Func<TEntity, object>> sortExpression)
        {
            var sorted = await this.repository.GetSortedDesc(Extend(sortExpression));

            return sorted.Select(c => c.Data);
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

        private static Expression<Func<DynamicMongoEntity<TIdentity,TEntity>, TValue>> Extend<TValue>
            (Expression<Func<TEntity,TValue>> source)
        {
            return source.ReplaceParameter((DynamicMongoEntity<TIdentity, TEntity> e) => e.Data);
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
