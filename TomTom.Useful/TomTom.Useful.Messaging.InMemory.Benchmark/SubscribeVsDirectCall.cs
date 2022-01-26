using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging.InMemory.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
    [RankColumn]
    public class SubscribeVsDirectCall
    {
        private static readonly InMemoryMessageBus<DummyMessage> bus;
        private static readonly DummyMessageHandler handler = new ();
        static SubscribeVsDirectCall()
        {
            bus = new InMemoryMessageBus<DummyMessage>(
                new InMemoryBusSettings { LogWholeMessageOnFault = false }
                , new EmptyLogger()
                , new Serializers.Json.JsonSerializer<DummyMessage>());


            bus.Subscribe(handler.Handle);
        }

        [Benchmark]
        public async Task DirectCall()
        {
            await handler.Handle(new DummyMessage(), new InMemoryMessageBus<DummyMessage>.Context());
        }

        [Benchmark]
        public async Task Publish()
        {
            await bus.Publish(new DummyMessage());
        }
    }
}
