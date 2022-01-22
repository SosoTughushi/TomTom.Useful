using TomTom.Useful.DataTypes;

namespace TomTom.Useful.EventSourcing.CommandHandling
{
    public class AggregateCommandHandlerResult<TAggregateIdentity, TRejectionReason> : Result<IEnumerable<Event<TAggregateIdentity>>, TRejectionReason>
    {
        public AggregateCommandHandlerResult(IEnumerable<Event<TAggregateIdentity>> value) : base(value)
        {
        }

        public AggregateCommandHandlerResult(TRejectionReason error) : base(error)
        {
        }
    }
}
