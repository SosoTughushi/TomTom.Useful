using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.DataTypes;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.EventSourcing
{
    public partial class AggregateCommandHandlers<TAggregate, TAggregateIdentity, TValidationError> : IHostedService
        where TAggregate : IAggregate<TAggregateIdentity>
    {
        private readonly ISubscriber<ICommand<TAggregateIdentity>> subscriber;
        private readonly IEntityByKeyProvider<TAggregateIdentity, TAggregate> aggregateRepository;
        private readonly IEventPublisher publisher;

        private Func<ICommand<TAggregateIdentity>, TAggregate, AggregateCommandHandlerResult> modifyHandler;
        private Func<ICommand<TAggregateIdentity>, CreateAggregateCommandHandlerResult?> createHandler;
        private IAsyncDisposable? subscription;

        public AggregateCommandHandlers(
            ISubscriber<ICommand<TAggregateIdentity>> subscriber,
            IEntityByKeyProvider<TAggregateIdentity, TAggregate> aggregateRepository,
            IEventPublisher publisher)
        {
            this.subscriber = subscriber;
            this.aggregateRepository = aggregateRepository;
            this.publisher = publisher;

            modifyHandler = (command, _) =>
            {
                throw new InvalidOperationException($"No handler registered for {command.GetType().FullName} command type.");
            };
            createHandler = command => null;
        }
        protected void RegisterCreateCommandHandler<TCommand>(Func<TCommand, CreateAggregateCommandHandlerResult> handler)
            where TCommand : ICommand<TAggregateIdentity>
        {
            if (createHandler == null)
            {
                createHandler = command =>
                {
                    if (command is TCommand tCommand)
                    {
                        return handler(tCommand);
                    }

                    return null;
                };
            }
        }

        protected void RegisterCommandHandler<TCommand>(Func<TCommand, TAggregate, AggregateCommandHandlerResult> handler)
        {
            var prev = modifyHandler;

            modifyHandler = (command, aggregate) =>
            {
                if (command is TCommand tCommand)
                {
                    return handler(tCommand, aggregate);
                }
                return prev(command, aggregate);
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.subscription = await subscriber.Subscribe(OnCommand);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (subscription != null)
            {
                await subscription.DisposeAsync();
            }
        }
        protected virtual Task OnValidationError(TValidationError error, ICommand<TAggregateIdentity> command, TAggregate? aggregate = default(TAggregate))
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnSuccessfullHandle(ICommand<TAggregateIdentity> command, TAggregate aggregate)
        {
            return Task.CompletedTask;
        }

        private async Task OnCommand(ICommand<TAggregateIdentity> command, ICurrentMessageContext context)
        {
            var createdResult = this.createHandler(command);
            if (createdResult != null)
            {
                if (createdResult.Success)
                {
                    var (newAggregate, events) = createdResult.Value;
                    await this.publisher.Publish(events);
                    await context.Ack();
                }
                else
                {
                    await context.Nack(createdResult.Error);
                    await OnValidationError(createdResult.Error, command);
                }

                return;
            }

            var aggregate = await this.aggregateRepository.Get(command.TargetIdentity);

            if (aggregate == null)
            {
                throw new InvalidOperationException($"Aggregate of type {typeof(TAggregate)} with Identity='{command.TargetIdentity}' does not exist.");
            }

            var modifyResult = this.modifyHandler(command, aggregate);

            if (modifyResult.Success)
            {
                var events = modifyResult.Value;
                await this.publisher.Publish(events);
                await context.Ack();
            }
            else
            {
                await context.Nack(modifyResult.Error);
                await OnSuccessfullHandle(modifyResult.Error, command, aggregate);
            }
        }

        #region Result Classes

        public class CreateAggregateCommandHandlerResult :
            Result<(TAggregate, IEnumerable<Event<TAggregateIdentity>>), TValidationError>
        {
            public CreateAggregateCommandHandlerResult(TValidationError error) : base(error)
            {
            }

            public CreateAggregateCommandHandlerResult(TAggregate aggregate, IEnumerable<Event<TAggregateIdentity>> events) : base((aggregate, events))
            {
            }
        }

        public class AggregateCommandHandlerResult : Result<IEnumerable<Event<TAggregate>>, TValidationError>
        {
            public AggregateCommandHandlerResult(IEnumerable<Event<TAggregate>> value) : base(value)
            {
            }

            public AggregateCommandHandlerResult(TValidationError error) : base(error)
            {
            }
        }

        #endregion
    }
}
