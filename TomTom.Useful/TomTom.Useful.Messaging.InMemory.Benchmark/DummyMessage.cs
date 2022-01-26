using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging.InMemory.Benchmark
{
    public class DummyMessage : IMessage
    {
        public Guid MessageId { get; } = Guid.Empty;

        public string CorrelationId { get; } = string.Empty;

        public string CausedById { get; set; } = string.Empty;
    }
}
