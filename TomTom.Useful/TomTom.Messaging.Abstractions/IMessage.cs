namespace TomTom.Useful.Messaging
{
    public interface IMessage 
    {
        public Guid MessageId { get; }
        public string CorrelationId { get;  }

        public string CausedById { get; }
    }
}