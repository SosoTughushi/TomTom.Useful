using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public class SomeMongoRepository : MongoRepository<string, SomeMongoEntity>
    {
        public SomeMongoRepository(MongoConifgurations conifgurations, IMongoDatabase database) : base(conifgurations, database)
        {
        }
    }
}
