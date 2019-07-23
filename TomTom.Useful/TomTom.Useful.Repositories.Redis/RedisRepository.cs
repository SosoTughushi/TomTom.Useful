using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Redis
{
    public class RedisRepository<TEntity> :
        IPurger<TEntity>,
        IListProvider<TEntity>,
        ICrud<string, TEntity>
        where TEntity: RedisEntity
    {
        private readonly IRedisStorage<TEntity> storage;
        protected ResultFactory<object> resultFactory = Result.GetFactory<object>();

        public RedisRepository(IRedisStorage<TEntity> storage)
        {
            this.storage = storage;
        }

        public async Task<Result<object>> Delete(string identity)
        {
            await this.storage.Delete(identity);

            return resultFactory.Ok();
        }

        public Task<TEntity> Get(string identity)
        {
            return this.storage.Get(identity);
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return this.storage.GetAll();
        }

        public async Task<Result<object>> Insert(TEntity entity)
        {
            await this.storage.Insert(entity);
            return this.resultFactory.Ok();
        }

        public async Task<Result<object>> Purge()
        {
            await this.storage.Purge();
            return this.resultFactory.Ok();
        }

        public async Task<Result<object>> Update(TEntity entity)
        {
            await this.storage.Update(entity);
            return this.resultFactory.Ok();
        }
    }
}
