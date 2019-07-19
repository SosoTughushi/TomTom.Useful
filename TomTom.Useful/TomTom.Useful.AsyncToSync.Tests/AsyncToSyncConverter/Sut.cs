using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.AsyncToSync.Tests.AsyncToSyncConverter
{
    public class Sut : AsyncToSyncConverter<string, Something>
    {
        public Sut(int timeoutInMs = 500)
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
