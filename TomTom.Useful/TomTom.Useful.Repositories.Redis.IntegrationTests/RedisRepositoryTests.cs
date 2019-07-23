using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;
using TomTom.Useful.Serializers.Json;
using Xunit;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class RedisRepositoryTests : RedisRepositoryTestBase
    {
        public RedisRepositoryTests()
        {
        }

        protected override RedisRepositoryConfigurator RegisterRepositories(RedisRepositoryConfigurator builder)
        {
            return builder.RegisterRepository<RedisIntegrationRepository, RedisEntityForIntegration>();
        }

        [Fact]
        public async Task Should_create_single_entity()
        {
            // arrange
            var entity = new RedisEntityForIntegration
            {
                Age = 28,
                Email = "mryemama@gmail.com",
                Id = "Soso",
                Username = "TomTom"
            };

            // act
            var result = await this.GetWriter()
                .Insert(entity);

            // assert
            var savedEntity = await this.GetSingleKeyProvider()
                .Get(entity.Id);

            Assert.Equal(entity.Id, savedEntity.Id);
        }

        [Fact]
        public async Task Should_update_single_entity()
        {
            // arrange
            var entity = new RedisEntityForIntegration
            {
                Age = 28,
                Email = "mryemama@gmail.com",
                Id = "SosoToUpdate",
                Username = "SouSou"
            };

            var result = await GetWriter()
                .Insert(entity);

            entity.Age = 27;
            // act
            result = await GetWriter()
                .Update(entity);

            // assert
            var savedEntity = await this.GetSingleKeyProvider()
                .Get(entity.Id);

            Assert.Equal(entity.Id, savedEntity.Id);
            Assert.Equal(27, savedEntity.Age);
        }

        [Fact]
        public async Task Should_remove_single_entity()
        {

            // arrange
            var entity = new RedisEntityForIntegration
            {
                Age = 28,
                Email = "mryemama@gmail.com",
                Id = "SosoToDelete",
                Username = "SouSou"
            };

            var result = await this.GetWriter().Insert(entity);

            entity.Age = 27;
            // act
            result = await this.GetWriter().Delete(entity.Id);

            // assert
            var savedEntity = await this.GetSingleKeyProvider().Get(entity.Id);

            Assert.Null(savedEntity);
        }

        [Fact]
        public async Task Should_get_all_entities()
        {
            // arrange
            await this.Cleanup();

            var entities = Enumerable.Range(0, 100)
                .Select(c => new RedisEntityForIntegration
                {
                    Age = c,
                    Email = c.ToString(),
                    Id = c.ToString(),
                    Username = c.ToString()
                }).ToList();

            foreach (var entity in entities)
            {
                await this.GetWriter().Insert(entity);
            }

            // act
            var resultsFromDb = await this.provider.GetService<IListProvider<RedisEntityForIntegration>>()
                .GetAll();

            // assert
            Assert.Equal(entities.Select(c => c.Id), resultsFromDb.Select(c => c.Id));
        }


        protected override Task Cleanup()
        {
            return this.provider.GetService<IPurger<RedisEntityForIntegration>>().Purge();
        }


        private IWriter<string, RedisEntityForIntegration> GetWriter()
        {
            return this.provider.GetService<IWriter<string, RedisEntityForIntegration>>();
        }

        private IEntityByKeyProvider<string,RedisEntityForIntegration> GetSingleKeyProvider()
        {
            return this.provider.GetService<IEntityByKeyProvider<string, RedisEntityForIntegration>>();
        }
    }
}
