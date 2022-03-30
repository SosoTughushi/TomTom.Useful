using MediatR;
using TomTom.Useful.Demo.Application.Commands;

namespace TomTom.Useful.Demo.Application.CommandHandlers
{
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : CommandBase<TResponse>
    {
    }
}
