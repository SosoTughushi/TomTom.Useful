using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes.Serialization;

namespace TomTom.Useful.Repositories.Redis
{
    public class RedisStorage<T> : IRedisStorage<T> where T : RedisEntity
    {
        private readonly IDatabase database;
        private readonly ISerializer<T> serializer;
        private readonly IDeserializer<T> deserializer;

        private readonly string @namespace;

        private readonly string keyForIds;

        private readonly string keyForSequence;

        public RedisStorage(IConnectionMultiplexer client,
            ISerializer<T> serializer,
            IDeserializer<T> deserializer,
            string @namespace)
        {
            database = client.GetDatabase();
            this.serializer = serializer;
            this.deserializer = deserializer;

            this.@namespace = @namespace;
            keyForIds = $"ids:{@namespace}";
            keyForSequence = $"seq:{@namespace}";
        }

        public RedisStorage(IConnectionMultiplexer client,
            ISerializer<T> serializer,
            IDeserializer<T> deserializer)
            : this(client, serializer, deserializer, typeof(T).FullName)
        {
        }

        public async Task<long> GetNextSequence()
        {
            return await database.StringIncrementAsync(keyForSequence);
        }

        public async Task Insert(T item)
        {
            var key = MakeKey(item.Id);

            if (await database.StringSetAsync(key, serializer.Serialize(item)))
            {
                await database.SetAddAsync(keyForIds, item.Id);
            }
        }

        public async Task Update(T item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                throw new InvalidOperationException($"Id: {item.Id} is not a valid id to update the item.");
            }

            var key = MakeKey(item.Id);

            await database.StringSetAsync(key, serializer.Serialize(item));
        }

        public async Task<IEnumerable<T>> GetAll()
        { 

            var ids = await database.SetMembersAsync(keyForIds);

            var rows = await database.StringGetAsync(ids.Select(id => (RedisKey)MakeKey(id)).ToArray());

            return rows
                .Where(item => item.HasValue)
                .Select(item => deserializer.Deserialize(item));
        }

        public async Task<T> Get(string id)
        {
            var key = MakeKey(id);

            if (await database.SetContainsAsync(keyForIds, id))
            {
                var value = await database.StringGetAsync(key);

                if (value.IsNullOrEmpty)
                {
                    return default(T);
                }

                return deserializer.Deserialize(value);
            }
            else
            {
                return default(T);
            }
        }

        public async Task Purge()
        {
            var ids = await database.SetMembersAsync(keyForIds);

            if (ids.Length > 0)
            {
                var keys = ids.Select(id => (RedisKey)MakeKey(id)).ToArray();

                await database.KeyDeleteAsync(keys);
                await database.SetRemoveAsync(keyForIds, ids);
            }
        }

        public async Task Delete(string id)
        {
            if (await database.KeyDeleteAsync(MakeKey(id)))
            {
                await database.SetRemoveAsync(keyForIds, id);
            }
        }

        private string MakeKey(string id)
        {
            return $"val:{@namespace}:{id}";
        }

        private string MakeKey(long id)
        {
            return MakeKey(id.ToString());
        }
    }
}
