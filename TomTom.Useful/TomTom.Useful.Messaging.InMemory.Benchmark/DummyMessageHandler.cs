using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging.InMemory.Benchmark
{
    public class DummyMessageHandler
    {
        public Task Handle(DummyMessage message, ICurrentMessageContext context)
        {
            return Task.CompletedTask;
        }
    }
}
