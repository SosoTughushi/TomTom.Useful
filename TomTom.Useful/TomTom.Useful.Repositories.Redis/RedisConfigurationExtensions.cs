using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TomTom.Useful.DataTypes.Serialization;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Redis
{
    public static class RedisConfigurationExtensions
    {
        public static void AddRedisRepositories(this IServiceCollection collection, string configuration, Action<RedisRepositoryBuilder> registerRepositories)
        {
            var config = ConfigurationOptions.Parse(configuration);
            collection.AddRedisRepositories(config, registerRepositories);
        }

        public static void AddRedisRepositories(this IServiceCollection collection, ConfigurationOptions configuration, Action<RedisRepositoryBuilder> registerRepositories)
        {
            collection.AddSingleton(c => InitializeConnection(configuration));

            var builder = new RedisRepositoryBuilder(collection);

            registerRepositories(builder);
        }

        private static IConnectionMultiplexer InitializeConnection(ConfigurationOptions config)
        {
            foreach (var endpoint in config.EndPoints)
            {
                var addressEndpoint = endpoint as DnsEndPoint;
                if (addressEndpoint == null)
                {
                    continue;
                }

                var port = addressEndpoint.Port;

                var isIp = IsIpAddress(addressEndpoint.Host);
                if (!isIp)
                {
                    //Please Don't use this line in blocking context. Please remove ".Result"
                    //Just for test purposes
                    IPHostEntry ip = Dns.GetHostEntryAsync(addressEndpoint.Host).Result;
                    config.EndPoints.Remove(addressEndpoint);
                    config.EndPoints.Add(ip.AddressList.First(), port);
                }
            }

            return ConnectionMultiplexer.Connect(config);
        }

        private static bool IsIpAddress(string host)
        {
            string ipPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
            return Regex.IsMatch(host, ipPattern);
        }

        public class RedisRepositoryBuilder
        {
            private readonly IServiceCollection collection;

            public RedisRepositoryBuilder(IServiceCollection collection)
            {
                this.collection = collection;
            }

            public RedisRepositoryBuilder RegisterRepository<TRepository, TEntity>(string @namespace = null, Func<IServiceProvider, TRepository> factory = null)
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

            public RedisRepositoryBuilder RegisterDynamicRepository<TIdentity, TEntity>(
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

            private void RegisterRepositoryInterfaces<TRepository, TIdentity, TEntity>()
                where TRepository : IKeyValueRepository<TIdentity, TEntity>, IPurger<TEntity>
            {
                this.collection.AddTransient<IPurger<TEntity>>(provider => provider.GetService<TRepository>());
                this.collection.AddTransient<IKeyValueRepository<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
                this.collection.AddTransient<IWriter<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
                this.collection.AddTransient<IEntityByKeyProvider<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            }
        }
    }
}
