using MediatR;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.Demo.Application.Commands
{
    public class CommandBase<TResult> : IRequest<TResult>, IMessage
    {
        public CommandBase(DemoRequestContext context)
        {
            MessageId = Guid.NewGuid();
            CorrelationId = context.CorrelationId;
            CausedById = context.RequestId;
            CreationTimestamp = context.Timestamp;
        }

        public Guid MessageId { get; }

        public string CorrelationId { get; }

        public string CausedById { get; }

        public DateTime CreationTimestamp { get; }
    }
}
