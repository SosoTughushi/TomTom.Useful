using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.AsyncToSync.Tests
{
    public class AsyncToSyncConverterSut : AsyncToSyncConverter<string, Something>
    {
        public AsyncToSyncConverterSut(int timeoutInMs = 500)
            : base(timeoutInMs, c=>c.CorrelationId)
        {
        }

    }

    public class Something
    {
        public string CorrelationId { get; set; }
        public int Number { get; internal set; }
    }
}
