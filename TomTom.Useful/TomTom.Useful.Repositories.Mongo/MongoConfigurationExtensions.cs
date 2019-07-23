using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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

            var mongoUrlObject = new MongoUrl(configurations.MongoUrl);
            var client = new MongoClient(mongoUrlObject);
            var database = client.GetDatabase(mongoUrlObject.DatabaseName);
            collection.AddSingleton(database);

            registrator(builder);
        }

    }
}
