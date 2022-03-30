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
        public QueryBase(DemoRequestContext context)
        {
            MessageId = Guid.NewGuid();
            CorrelationId = context.CorrelationId;
            CausedById = context.RequestId;
            Context = context;
        }

        public Guid MessageId { get; }

        public string CorrelationId { get; }

        public string CausedById { get; }
        public DemoRequestContext Context { get; }
    }
}
