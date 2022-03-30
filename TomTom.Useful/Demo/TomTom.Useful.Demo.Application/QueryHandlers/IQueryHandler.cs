using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Demo.Application.Queries;

namespace TomTom.Useful.Demo.Application.QueryHandlers
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : QueryBase<TResponse>
    {
    }
}
