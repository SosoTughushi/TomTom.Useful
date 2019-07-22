using System.Collections.Generic;
using System.Threading.Tasks;

namespace TomTom.Useful.Repositories.Redis
{
    public interface IRedisStorage<T> where T : RedisEntity
    {
        Task Delete(string id);
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
        Task<long> GetNextSequence();
        Task Insert(T item);
        Task Purge();
        Task Update(T item);
    }
}