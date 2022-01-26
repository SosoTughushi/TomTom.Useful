using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Application.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
    [RankColumn]
    public class EndToEndPlaylistBenchmark
    {
        static EndToEndPlaylistBenchmark()
        {
            var app = new DemoApplication(new string[0]);

            app.Run();

            App = app;
        }

        private static readonly DemoApplication App;
        private static readonly DemoRequestContext context = new DemoRequestContext(
            Guid.NewGuid().ToString()
            , Guid.NewGuid().ToString().ToString()
            , Guid.NewGuid());


        [Benchmark]
        public async Task CreatePlaylist()
        {
            await App.PlaylistWriter.Create(Guid.NewGuid().ToString(), context);
        }
    }
}
