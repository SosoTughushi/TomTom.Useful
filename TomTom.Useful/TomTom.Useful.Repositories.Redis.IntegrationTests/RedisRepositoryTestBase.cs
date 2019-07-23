using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Serializers.Json;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public abstract class RedisRepositoryTestBase : IDisposable
    {
        protected IServiceProvider provider;

        public RedisRepositoryTestBase()
        {
            var services = new ServiceCollection();
            var configurations = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();

            services.AddRedisRepositories(configurations["redisClient"],
                builder => RegisterRepositories(builder));

            services.AddJsonSerialization();

            this.provider = services.BuildServiceProvider();
        }

        protected abstract RedisRepositoryConfigurator RegisterRepositories(RedisRepositoryConfigurator builder);

        protected abstract Task Cleanup();

        public void Dispose()
        {
            Cleanup().Wait();
        }
    }
}
