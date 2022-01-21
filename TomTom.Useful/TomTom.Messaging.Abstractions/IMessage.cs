namespace TomTom.Useful.Messaging
{
    public interface IMessage 
    {
        public Guid Id { get; }
        public string CorrelationId { get;  }

        public string CausedById { get; }
    }
}