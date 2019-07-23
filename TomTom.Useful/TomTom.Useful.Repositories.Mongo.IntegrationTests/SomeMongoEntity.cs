using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public class SomeMongoEntity : MongoEntity<string>
    {
        public string Name { get; set; }
    }
}
