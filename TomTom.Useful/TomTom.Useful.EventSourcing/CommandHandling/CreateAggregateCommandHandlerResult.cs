using TomTom.Useful.DataTypes;

namespace TomTom.Useful.EventSourcing.CommandHandling
{
    public class CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TRejectionReason> :
        Result<(TAggregate, IEnumerable<Event<TAggregateIdentity>>), TRejectionReason>
    {
        public CreateAggregateCommandHandlerResult(TRejectionReason error) : base(error)
        {
        }

        public CreateAggregateCommandHandlerResult(TAggregate aggregate, IEnumerable<Event<TAggregateIdentity>> events) : base((aggregate, events))
        {
        }
    }
}
