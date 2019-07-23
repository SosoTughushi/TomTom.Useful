using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public abstract class MongoRepositoryTestBase : IDisposable
    {
        protected readonly ServiceProvider provider;

        public MongoRepositoryTestBase()
        {
            var services = new ServiceCollection();
            var configurations = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();

            services.AddMongoRepositories(new MongoConifgurations
            {
                MongoUrl = configurations["mongoConnString"],
                Schema = "IntegrationTests"
            },
                builder => RegisterRepositories(builder));

            this.provider = services.BuildServiceProvider();
        }
        protected abstract MongoRepositoryConfigurator RegisterRepositories(MongoRepositoryConfigurator builder);

        protected abstract Task Cleanup();

        public void Dispose()
        {
            this.Cleanup().Wait();
        }
    }
}
