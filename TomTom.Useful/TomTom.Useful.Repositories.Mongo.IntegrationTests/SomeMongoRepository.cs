using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public class SomeMongoRepository : MongoRepository<string, SomeMongoEntity>
    {
        public SomeMongoRepository(MongoConfigurations configurations, IMongoDatabase database) : base(configurations, database)
        {
        }
    }
}
