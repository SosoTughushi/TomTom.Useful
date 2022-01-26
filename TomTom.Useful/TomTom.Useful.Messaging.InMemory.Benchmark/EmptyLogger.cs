using Microsoft.Extensions.Logging;

namespace TomTom.Useful.Messaging.InMemory.Benchmark
{
    class EmptyLogger : ILogger<InMemoryMessageBus<DummyMessage>>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
