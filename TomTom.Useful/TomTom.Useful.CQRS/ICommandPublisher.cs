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

    public static class CommandPublisherExtensions
    {
        public static ICommandPublisher<TCommand> AsCommandPublisher<TCommand>(this IPublisher<TCommand> publisher)
            where TCommand: ICommand
        {
            return new CommandPublisherAdapter<TCommand>(publisher);
        }

        private class CommandPublisherAdapter<TCommand> : ICommandPublisher<TCommand>
            where TCommand: ICommand
        {
            private readonly IPublisher<TCommand> publisher;

            public CommandPublisherAdapter(IPublisher<TCommand> publisher)
            {
                this.publisher = publisher;
            }

            public Task Publish(TCommand message)
            {
                return this.publisher.Publish(message);
            }

            public Task Publish(IEnumerable<TCommand> messages)
            {
                return this.publisher.Publish(messages);
            }
        }
    }
}
