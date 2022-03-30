using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.Demo.Application.Queries
{
    public class QueryBase<TResponse> : IRequest<TResponse>, IMessage
    {
        public QueryBase(Guid messageId, string correlationId, string causedById)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            CausedById = causedById;
        }

        public Guid MessageId { get; }

        public string CorrelationId { get; }

        public string CausedById { get; }
    }
}
