using TomTom.Useful.DataTypes;

namespace TomTom.Useful.CQRS
{
    public interface ICommandHandler<TCommand, TResponse> 
        where TCommand : ICommand
        where TResponse : Result
    {
        Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
