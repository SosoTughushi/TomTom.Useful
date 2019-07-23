using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Serializers.Json;
using Xunit;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class RedisRepositoryTests : IDisposable
    {
        private IServiceProvider provider;
        private RedisIntegrationRepository repository;
        public RedisRepositoryTests()
        {
            var services = new ServiceCollection();
            var configurations = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();

            services.AddRedisRepositories(configurations["redisClient"],
                builder =>
                builder.RegisterRepository<RedisIntegrationRepository,RedisEntityForIntegration>());

            services.AddJsonSerialization();

            this.provider = services.BuildServiceProvider();
            this.repository = provider.GetService<RedisIntegrationRepository>();
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
            var result = await this.repository.Insert(entity);

            // assert
            var savedEntity = await this.repository.Get(entity.Id);

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

            var result = await this.repository.Insert(entity);

            entity.Age = 27;
            // act
            result = await this.repository.Update(entity);

            // assert
            var savedEntity = await this.repository.Get(entity.Id);

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

            var result = await this.repository.Insert(entity);

            entity.Age = 27;
            // act
            result = await this.repository.Delete(entity.Id);

            // assert
            var savedEntity = await this.repository.Get(entity.Id);

            Assert.Null(savedEntity);
        }



        public void Dispose()
        {
            this.repository.Purge().Wait();
        }
    }
}
