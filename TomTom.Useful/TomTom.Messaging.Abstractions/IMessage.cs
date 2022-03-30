namespace TomTom.Useful.Messaging
{
    public interface IMessage 
    {
        public Guid MessageId { get; }
        public string CorrelationId { get;  }

        public string CausedById { get; }
    }

    public class MessageMetadata
    {
        private MessageMetadata(Guid messageId, string correlationId, string causedById)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            CausedById = causedById;
        }

        public Guid MessageId { get; }
        public string CorrelationId { get; }

        public string CausedById { get; }

        public MessageMetadata Chain()
        {
            return CreateNew(this.CorrelationId, this.MessageId.ToString());
        }

        public static MessageMetadata CreateNew(string correlationId, string causationId)
        {
            var id = Guid.NewGuid();
            return new MessageMetadata(id, correlationId, causationId);
        }
    }
}