using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using TomTom.Useful.DataTypes.Serialization;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Redis
{
    public class RedisRepositoryConfigurator
    {
        private readonly IServiceCollection collection;

        public RedisRepositoryConfigurator(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public RedisRepositoryConfigurator RegisterRepository<TRepository, TEntity>(string @namespace = null, Func<IServiceProvider, TRepository> factory = null)
            where TRepository : RedisRepository<TEntity>
            where TEntity : RedisEntity
        {
            if (@namespace != null)
            {
                collection.AddSingleton<IRedisStorage<TEntity>>(provider =>
                    new RedisStorage<TEntity>(
                        provider.GetService<IConnectionMultiplexer>(),
                        provider.GetService<ISerializer<TEntity>>(),
                        provider.GetService<IDeserializer<TEntity>>(),
                        @namespace));
            }
            else
            {
                collection.AddSingleton<IRedisStorage<TEntity>, RedisStorage<TEntity>>();
            }

            if (factory != null)
            {
                collection.AddTransient(provider => factory(provider));
            }
            else
            {
                collection.AddTransient<TRepository>();
            }

            RegisterRepositoryInterfaces<TRepository, string, TEntity>();

            return this;
        }

        public RedisRepositoryConfigurator RegisterDynamicRepository<TIdentity, TEntity>(
            Func<TEntity, TIdentity> keyExtractor,
            Func<TIdentity, string> identityToString,
            string @namespace = null)
            where TEntity : class
        {
            RegisterRepository<RedisRepository<DynamicRedisEntity<TEntity>>, DynamicRedisEntity<TEntity>>(@namespace);

            collection.AddTransient(provider =>
                new DynamicRedisRepository<TIdentity, TEntity>(
                    provider.GetService<RedisRepository<DynamicRedisEntity<TEntity>>>(),
                    keyExtractor,
                    identityToString));

            RegisterRepositoryInterfaces<DynamicRedisRepository<TIdentity, TEntity>, TIdentity, TEntity>();

            return this;
        }

        public RedisRepositoryConfigurator RegisterDynamicRepository<TEntity>(
            Func<TEntity, string> keyExtractor,
            string @namespace = null)
            where TEntity: class
        {
            return RegisterDynamicRepository<string, TEntity>(keyExtractor, c => c, @namespace);
        }

        private void RegisterRepositoryInterfaces<TRepository, TIdentity, TEntity>()
            where TRepository : IKeyValueRepository<TIdentity, TEntity>, IPurger<TEntity>, IListProvider<TEntity>
        {
            this.collection.AddTransient<IPurger<TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IKeyValueRepository<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IWriter<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IEntityByKeyProvider<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IListProvider<TEntity>>(provider => provider.GetService<TRepository>());
        }
    }
}
