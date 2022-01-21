using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Domain.Identities;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Domain.Dal.InMemory
{
    public static class DomainDalInMemoryExtensions
    {
        public static void AddDemoDomainInMemoryRepositories(this IServiceCollection services)
        {
            services.AddSingleton<PlaylistInMemoryRepository>();

            services.AddSingleton<IHostedService>(provider => provider.GetService<PlaylistInMemoryRepository>());

            services.AddTransient<IEntityByKeyProvider<PlaylistIdentity, Playlist.Playlist?>>(provider => provider.GetService<PlaylistInMemoryRepository>());
        }
    }
}
