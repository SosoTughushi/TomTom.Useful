using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.Messaging;

namespace TomTom.Useful.CQRS
{
    public interface ICommandPublisher <TCommand> : IPublisher<TCommand>
        where TCommand : ICommand
    {
    }
}
