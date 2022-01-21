namespace TomTom.Useful.Demo.Application
{
    public class DemoAppContext
    {
        public DemoAppContext(string correlationId, string requestId, Guid currentUserId)
        {
            CorrelationId = correlationId;
            RequestId = requestId;
            CurrentUserId = currentUserId;
        }

        public string CorrelationId { get; }
        public string RequestId { get; }
        public Guid CurrentUserId { get; }
    }
}