using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Mongo
{
    public static class MongoConfigurationExtensions
    {
        public static void AddMongoRepositories(this IServiceCollection collection,
            MongoConifgurations configurations, Action<MongoRepositoryConfigurator> registrator)
        {
            var builder = new MongoRepositoryConfigurator(collection, configurations);

            registrator(builder);
        }

    }
}
