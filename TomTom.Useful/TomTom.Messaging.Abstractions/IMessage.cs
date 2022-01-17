namespace TomTom.Useful.Messaging
{
    public interface IMessage 
    {
        public Guid? Id { get; }
        public Guid CorrelationId { get;  }
        public Guid? CausationId { get; }
    }
}