using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class DynamicRedisRepositoryTests : RedisRepositoryTestBase
    {
        [Fact]
        public async Task Should_insert_poco()
        {
            // arrange
            var poco = new RedisPoco
            {
                Id = 15,
                Inner = new RedisPoco.InnerPoco
                {
                    Child = "Child",
                    Rand = 1241
                },
                Name = "Name",
                Prioriy = 48
            };

            // act
            await GetWriter().Insert(poco);

            // assert
            var inserted = await GetSingleKyeProvider().Get(poco.Id);

            Assert.Equal(poco.Id, inserted.Id);
            Assert.Equal(poco.Inner.Child, inserted.Inner.Child);
        }

        [Fact]
        public async Task Should_update_poco()
        {
            // arrange
            var poco = new RedisPoco
            {
                Id = 99,
                Inner = new RedisPoco.InnerPoco
                {
                    Child = "Child",
                    Rand = 19888446
                },
                Name = "Name",
                Prioriy = 88
            };

            await this.GetWriter().Insert(poco);

            // act
            poco.Inner.Rand = 4895;
            await this.GetWriter().Update(poco);

            // assert
            var updated = await this.GetSingleKyeProvider().Get(poco.Id);
            Assert.Equal(poco.Inner.Rand, updated.Inner.Rand);
            
        }

        [Fact]
        public async Task Should_delete_poco()
        {
            // arrange
            var poco = new RedisPoco
            {
                Id = 189
            };

            await this.GetWriter().Insert(poco);

            // act
            await this.GetWriter().Delete(poco.Id);

            // assert
            var deleted = await this.GetSingleKyeProvider().Get(poco.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Should_get_all()
        {
            // arrange
            await this.provider.GetService<IPurger<RedisPoco>>().Purge();
            var pocos = Enumerable.Range(0, 100)
                .Select(c=>new RedisPoco { Id = c });

            foreach(var poco in pocos)
            {
                await this.GetWriter().Insert(poco);
            }

            // act
            var fromDb = await this.provider.GetService<IListProvider<RedisPoco>>().GetAll();

            // assert
            Assert.Equal(pocos.Select(c => c.Id), fromDb.Select(c => c.Id));
        }

        private IEntityByKeyProvider<int, RedisPoco> GetSingleKyeProvider()
        {
            return provider.GetService<IEntityByKeyProvider<int, RedisPoco>>();
        }

        private IWriter<int, RedisPoco> GetWriter()
        {
            return provider.GetService<IWriter<int, RedisPoco>>();
        }

        protected override Task Cleanup()
        {
            var service = provider.GetService<IPurger<RedisPoco>>();

            return service.Purge();
        }

        protected override RedisRepositoryConfigurator RegisterRepositories(RedisRepositoryConfigurator builder)
        {
            return builder.RegisterDynamicRepository<int, RedisPoco>(c => c.Id, id => id.ToString());
        }
    }
}
