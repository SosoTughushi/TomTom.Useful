using Microsoft.Extensions.DependencyInjection;
using System;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Repositories.Mongo
{
    public class MongoRepositoryConfigurator
    {
        private readonly IServiceCollection collection;
        private readonly MongoConifgurations configurations;

        public MongoRepositoryConfigurator(IServiceCollection collection, MongoConifgurations configurations)
        {
            this.collection = collection;
            this.configurations = configurations;
        }

        public MongoRepositoryConfigurator RegisterRepository<TRepository, TIdentity, TEntity>
            (Func<MongoConifgurations, IServiceProvider, TRepository> factory)
            where TRepository : MongoRepository<TIdentity, TEntity>
            where TEntity : MongoEntity<TIdentity>
        {
            this.collection.AddSingleton(provider => factory(this.configurations, provider));
            this.RegisterRepositoryInterfaces<TRepository, TIdentity, TEntity>();
            return this;
        }


        public MongoRepositoryConfigurator RegisterDynamicRepository<TIdentity, TEntity>(Func<TEntity, TIdentity> keyExtractor) where TEntity : class
        {
            this.RegisterRepository<
                MongoRepository<TIdentity, DynamicMongoEntity<TIdentity, TEntity>>,
                TIdentity,
                DynamicMongoEntity<TIdentity, TEntity>>((c, p) =>
                   new MongoRepository<TIdentity, DynamicMongoEntity<TIdentity, TEntity>>(this.configurations));

            this.collection.AddTransient(
                provider => new DynamicMongoRepository<TIdentity, TEntity>(
                    provider.GetService<MongoRepository<TIdentity, DynamicMongoEntity<TIdentity, TEntity>>>(),
                    keyExtractor));

            this.RegisterRepositoryInterfaces<DynamicMongoRepository<TIdentity, TEntity>, TIdentity, TEntity>();

            return this;
        }

        private void RegisterRepositoryInterfaces<TRepository, TIdentity, TEntity>()
            where TRepository : IKeyValueRepository<TIdentity, TEntity>, IPurger<TEntity>
        {
            this.collection.AddTransient<IPurger<TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IKeyValueRepository<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IWriter<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
            this.collection.AddTransient<IEntityByKeyProvider<TIdentity, TEntity>>(provider => provider.GetService<TRepository>());
        }
    }
}
