using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace TomTom.Useful.Repositories.Mongo.IntegrationTests
{
    public class DynamicMongoRepositoryTests : MongoRepositoryTestBase
    {
        private readonly List<MongoPoco> cache;

        public DynamicMongoRepositoryTests()
        {
            // seet
            this.cache = Enumerable.Range(0, 100)
                .Select(c => new MongoPoco
                {
                    AverageRating = new Random().Next(),
                    Identity = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    Name = c.ToString(),
                    Inner = new MongoPoco.InnerPoco
                    {
                        Address = "Tsinamdzgvrishvili " + c,
                        City = "Tbilisi",
                        CountryId = c
                    }
                }).ToList();

            foreach(var item in this.cache)
            {
                this.GetWriter().Insert(item).Wait();
            }
        }

        [Fact]
        public async Task Should_get_all()
        {
            // act
            var itemsFromDb = await this.provider.GetService<IListProvider<MongoPoco>>().GetAll();

            // assert
            Assert.Equal(this.cache.Select(c => c.Identity), itemsFromDb.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_ordered()
        {
            // act
            var itemsFromDb = await this.provider
                .GetService<ISortedListProvider<MongoPoco>>()
                .GetSorted(p => p.AverageRating);

            // assert
            Assert.Equal(
                this.cache.OrderBy(p => p.AverageRating).Select(c => c.Identity),
                itemsFromDb.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_ordered_desc()
        {
            // act
            var itemsFromDb = await this.provider
                .GetService<ISortedListProvider<MongoPoco>>()
                .GetSortedDesc(p => p.AverageRating);

            // assert
            Assert.Equal(
                this.cache.OrderByDescending(p => p.AverageRating).Select(c => c.Identity),
                itemsFromDb.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_filtered()
        {
            // arrange
            var midValue = this.cache[this.cache.Count / 2].Inner.CountryId;

            // act
            var itemsFromDb = await this.provider
                .GetService<IFilteredListProvider<MongoPoco>>()
                .GetFiltered(c => c.Inner.CountryId > midValue);

            // assert
            Assert.Equal(
                this.cache.Where(c => c.Inner.CountryId > midValue).Select(c => c.Identity),
                itemsFromDb.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_filtered_and_sorted()
        {
            // arrange
            var midValue = this.cache[this.cache.Count / 2].Inner.CountryId;

            // act
            var itemsFromDb = await this.provider
                .GetService<IFilteredSortedListProvider<MongoPoco>>()
                .GetFilteredSorted(c => c.Inner.CountryId > midValue, c=>c.AverageRating);

            // assert
            Assert.Equal(
                this.cache.Where(c => c.Inner.CountryId > midValue).OrderBy(c=>c.AverageRating).Select(c => c.Identity),
                itemsFromDb.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_paged_result()
        {
            // act
            var result = await this.provider
                .GetService<IPagedListProvider<MongoPoco>>()
                .GetPaged(10, 15);

            // assert
            Assert.Equal(this.cache.Count, result.TotalSize);
            Assert.Equal(10, result.Offset);
            Assert.Equal(
                this.cache.Skip(10).Take(15).Select(c => c.Identity),
                result.Data.Select(c => c.Identity));
        }

        [Fact]
        public async Task Should_get_filtered_sorted_paged_result()
        {
            // arrange
            var midValue = this.cache[this.cache.Count / 2].Inner.CountryId;

            // act
            var result = await this.provider
                .GetService<IPagedFilteredSortedListProvider<MongoPoco>>()
                .GetPagedFilteredSorted(c => c.Inner.CountryId > midValue, c => c.AverageRating, 1, 3);

            // assert
            Assert.Equal(this.cache.Count(c => c.Inner.CountryId > midValue), result.TotalSize);
            Assert.Equal(3, result.Data.Count());
            Assert.Equal(1, result.Offset);
            Assert.Equal(
                this.cache
                .Where(c => c.Inner.CountryId > midValue)
                .OrderBy(c => c.AverageRating)
                .Skip(1)
                .Take(3)
                .Select(c => c.Identity),
                result.Data.Select(c => c.Identity));
        }

        protected override MongoRepositoryConfigurator RegisterRepositories(MongoRepositoryConfigurator builder)
        {
            return builder.RegisterDynamicRepository((MongoPoco p) => p.Composite);
        }

        protected override Task Cleanup()
        {
            return provider.GetService<IPurger<MongoPoco>>().Purge();
        }


        private IWriter<CompositeIdentity, MongoPoco> GetWriter() =>
            this.provider.GetService<IWriter<CompositeIdentity, MongoPoco>>();

        private IEntityByKeyProvider<CompositeIdentity, MongoPoco> GetKeyProvider() =>
            this.provider.GetService<IEntityByKeyProvider<CompositeIdentity, MongoPoco>>();
    }

    public class MongoPoco
    {
        public CompositeIdentity Composite => 
            new CompositeIdentity
            {
                Timestamp = Timestamp,
                Id = Identity
            };

        public DateTime Timestamp { get; set; }

        public Guid Identity { get; set; }

        public string Name { get; set; }

        public InnerPoco Inner { get; set; }

        public double AverageRating { get; set; }

        public class InnerPoco
        {
            public string City { get; set; }

            public int CountryId { get; set; }

            public string Address { get; set; }
        }
    }

    public class CompositeIdentity
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
