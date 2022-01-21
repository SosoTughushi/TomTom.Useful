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
    public abstract class AggregateCommandHandlers<TAggregate, TAggregateIdentity, TValidationError> : IHostedService
        where TAggregate : IAggregate<TAggregateIdentity>
    {
        private readonly ISubscriber<ICommand<TAggregateIdentity>> subscriber;
        private readonly IEntityByKeyProvider<TAggregateIdentity, TAggregate?> aggregateRepository;
        private readonly IEventPublisher publisher;

        private Func<ICommand<TAggregateIdentity>, TAggregate, AggregateCommandHandlerResult<TAggregateIdentity, TValidationError>> modifyHandler;
        private Func<ICommand<TAggregateIdentity>, CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TValidationError>?> createHandler;
        private IAsyncDisposable? subscription;

        protected AggregateCommandHandlers(
            ISubscriber<ICommand<TAggregateIdentity>> subscriber,
            IEntityByKeyProvider<TAggregateIdentity, TAggregate?> aggregateRepository,
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

        protected abstract void RegisterCommandHandlers();

        protected void RegisterCreateCommandHandler<TCommand>(Func<TCommand, CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TValidationError>> handler)
            where TCommand : ICommand<TAggregateIdentity>
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

        protected void RegisterCommandHandler<TCommand>(Func<TCommand, TAggregate, AggregateCommandHandlerResult<TAggregateIdentity, TValidationError>> handler)
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
            RegisterCommandHandlers();
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
                await OnSuccessfullHandle(command, aggregate);
            }
        }


    }

    #region Result Classes

    public class CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TValidationError> :
        Result<(TAggregate, IEnumerable<Event<TAggregateIdentity>>), TValidationError>
    {
        public CreateAggregateCommandHandlerResult(TValidationError error) : base(error)
        {
        }

        public CreateAggregateCommandHandlerResult(TAggregate aggregate, IEnumerable<Event<TAggregateIdentity>> events) : base((aggregate, events))
        {
        }
    }

    public class AggregateCommandHandlerResult<TAggregateIdentity, TValidationError> : Result<IEnumerable<Event<TAggregateIdentity>>, TValidationError>
    {
        public AggregateCommandHandlerResult(IEnumerable<Event<TAggregateIdentity>> value) : base(value)
        {
        }

        public AggregateCommandHandlerResult(TValidationError error) : base(error)
        {
        }
    }

    #endregion

    public interface ICreateAggregateCommandHandler<TAggregateIdentity, TAggregate, TCommand, TError>
        where TCommand : ICreateCommand<TAggregateIdentity>
        where TAggregate : IAggregate<TAggregateIdentity>
    {
        CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TError> Handle(TCommand command);
    }

    public interface IAggregateCommandHandler<TAggregateIdentity, TCommand, TAggregate, TError>
    {
        AggregateCommandHandlerResult<TAggregateIdentity, TError> Handle(TCommand command, TAggregate aggregate);
    }
}
