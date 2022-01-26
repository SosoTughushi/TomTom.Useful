using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.AsyncToSync.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
    [RankColumn]
    public class Dummy
    {
        private static AsyncToSyncConverter<Guid, Guid> converter = new AsyncToSyncConverter<Guid, Guid>(200, k => k);

        [Benchmark]
        public async Task MyStupidHead()
        {
            var resultGuid = Guid.NewGuid();

            var resultTask = converter.AwaitResult(resultGuid);
            converter.SetResult(resultGuid);

            await resultTask;
        }
    }
}
