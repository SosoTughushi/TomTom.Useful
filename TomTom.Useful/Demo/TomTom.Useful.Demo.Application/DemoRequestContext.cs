namespace TomTom.Useful.Demo.Application
{
    public class DemoRequestContext
    {
        public DemoRequestContext(string correlationId, string requestId, Guid currentUserId)
        {
            CorrelationId = correlationId;
            RequestId = requestId;
            CurrentUserId = currentUserId;
            Timestamp = DateTime.UtcNow;
        }

        public string CorrelationId { get; }
        public string RequestId { get; }
        public Guid CurrentUserId { get; }
        public DateTime Timestamp { get; }
    }
}
