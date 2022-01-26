using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Application.Playlist;

namespace TomTom.Useful.Demo.Application.Benchmark
{
    public class DemoApplication
    {
        private IHost host;

        public IPlaylistWriter PlaylistWriter { get; }
        public IPlaylistReader PlaylistReader { get; }

        public DemoApplication(string[] args)
        {
            this.host = Host.CreateDefaultBuilder(args)
               .ConfigureServices((_, services) =>
               {
                   services.AddDemoApplication();
               })
               .Build();

            this.PlaylistWriter = this.host.Services.GetService<IPlaylistWriter>() 
                ?? throw new InvalidOperationException($"{nameof(IPlaylistWriter)} is not registered in DI.");
            this.PlaylistReader = this.host.Services.GetService<IPlaylistReader>()
                ?? throw new InvalidOperationException($"{nameof(IPlaylistReader)} is not registered in DI.");
        }

        public async Task Run()
        {
            await host.RunAsync();
        }

        
    }
}
