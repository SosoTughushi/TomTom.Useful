using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public class MongoRepositoryTests : MongoRepositoryTestBase
    {
        protected override Task Cleanup()
        {
            return this.provider.GetService<IPurger<SomeMongoEntity>>().Purge();
        }

        [Fact]
        public async Task Should_insert_item()
        {
            // arrange
            var mongoEntity = new SomeMongoEntity
            {
                BsonId = "11",
                Name = "fqsfqfxfq1"
            };

            // act
            await this.GetWriter().Insert(mongoEntity);

            // assert
            var fromDb = await this.GetKeyProvider().Get(mongoEntity.BsonId);

            Assert.Equal(mongoEntity.Name, fromDb.Name);
        }

        [Fact]
        public async Task Should_update_item()
        {
            // arrange
            var entity = new SomeMongoEntity
            {
                BsonId = "BsonIdToUpdate",
                Name = "fq112"
            };

            await this.GetWriter().Insert(entity);

            // act
            entity.Name = "r1wrd21";

            await this.GetWriter().Update(entity);

            // assert
            var fromDb = await this.GetKeyProvider().Get(entity.BsonId);

            Assert.Equal(entity.Name, fromDb.Name);
        }

        [Fact]
        public async Task Should_delete_item()
        {
            // arrange
            var entity = new SomeMongoEntity
            {
                BsonId = "BsonIdToDelete",
                Name = "fq112"
            };

            await this.GetWriter().Insert(entity);

            // act

            await this.GetWriter().Delete(entity.BsonId);

            // assert
            var fromDb = await this.GetKeyProvider().Get(entity.BsonId);

            Assert.Null(fromDb);
        }

        private IWriter<string, SomeMongoEntity> GetWriter() =>
            this.provider.GetService<IWriter<string, SomeMongoEntity>>();

        private IEntityByKeyProvider<string, SomeMongoEntity> GetKeyProvider() =>
            this.provider.GetService<IEntityByKeyProvider<string, SomeMongoEntity>>();

        protected override MongoRepositoryConfigurator RegisterRepositories(MongoRepositoryConfigurator builder)
        {
            return builder
                .RegisterRepository<SomeMongoRepository, string, SomeMongoEntity>((config, provider) =>
                    new SomeMongoRepository(config, provider.GetService<IMongoDatabase>()));
        }
    }
}
