using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Mongo
{
    public class MongoEntity<TIdentity>
    {
		[BsonId]
        public TIdentity BsonId { get; set; }
    }
}
