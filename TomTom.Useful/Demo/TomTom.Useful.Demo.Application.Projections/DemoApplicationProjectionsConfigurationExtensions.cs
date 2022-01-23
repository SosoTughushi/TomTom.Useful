using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.Demo.Application.Projections
{
    public static class DemoApplicationProjectionsConfigurationExtensions
    {
        public static void AddDemoAppProjections(this IServiceCollection services)
        {
            services.AddSingleton<PlaylistProjection>();
            services.AddSingleton<IHostedService>(provider => provider.GetService<PlaylistProjection>());

            services.AddSingleton<ProjectedPlaylistInMemoryRepository>();
            services.AddSingleton<IWriter<Guid, ProjectedPlaylist>>(provider => provider.GetService<ProjectedPlaylistInMemoryRepository>());
            services.AddSingleton<IEntityByKeyProvider<Guid, ProjectedPlaylist?>>(provider => provider.GetService<ProjectedPlaylistInMemoryRepository>());
            services.AddSingleton<IEntityByKeyProvider<Guid, List<ProjectedPlaylist>>>(provider => provider.GetService<ProjectedPlaylistInMemoryRepository>());
        }
    }
}
