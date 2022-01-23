using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.CQRS;
using TomTom.Useful.Messaging;
using TomTom.Useful.Repositories.Abstractions;

namespace TomTom.Useful.EventSourcing.CommandHandling
{
    public abstract class AggregateCommandHandlers<TAggregate, TAggregateIdentity, TRejectionReason> : IHostedService
        where TAggregate : IAggregate<TAggregateIdentity>
    {
        private readonly ISubscriber<ICommand<TAggregateIdentity>> subscriber;
        private readonly IEntityByKeyProvider<TAggregateIdentity, TAggregate?> aggregateRepository;
        private readonly IEventPublisher<TAggregateIdentity> publisher;

        private Func<ICommand<TAggregateIdentity>, TAggregate, Task<AggregateCommandHandlerResult<TAggregateIdentity, TRejectionReason>>> modifyHandler;
        private Func<ICommand<TAggregateIdentity>, Task<CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TRejectionReason>?>> createHandler;
        private IAsyncDisposable? subscription;

        protected AggregateCommandHandlers(
            ISubscriber<ICommand<TAggregateIdentity>> subscriber,
            IEntityByKeyProvider<TAggregateIdentity, TAggregate?> aggregateRepository,
            IEventPublisher<TAggregateIdentity> publisher)
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

        protected void RegisterCreateCommandHandler<TCommand>(Func<TCommand, Task<CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TRejectionReason>>> handler)
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

        protected void RegisterCommandHandler<TCommand>(Func<TCommand, TAggregate, Task<AggregateCommandHandlerResult<TAggregateIdentity, TRejectionReason>>> handler)
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
            this.subscription = await subscriber.Subscribe(HandleCommand);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (subscription != null)
            {
                await subscription.DisposeAsync();
            }
        }

        protected virtual Task OnException(ICommand<TAggregateIdentity> command, Exception ex)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnCommandRejected(TRejectionReason error, ICommand<TAggregateIdentity> command, TAggregate? aggregate = default(TAggregate))
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnCommandSucceeded(ICommand<TAggregateIdentity> command, TAggregate aggregate)
        {
            return Task.CompletedTask;
        }

        private async Task HandleCommand(ICommand<TAggregateIdentity> command, ICurrentMessageContext context)
        {
            try
            {
                var createdResult = await this.createHandler(command);
                if (createdResult != null)
                {
                    if (createdResult.Success)
                    {
                        var (newAggregate, events) = createdResult.Value;
                        await this.publisher.Publish(events);
                        await context.Ack();
                        await OnCommandSucceeded(command, newAggregate);
                    }
                    else
                    {
                        await context.Nack(createdResult.Error);
                        await OnCommandRejected(createdResult.Error, command);
                    }

                    return;
                }

                var aggregate = await this.aggregateRepository.Get(command.TargetIdentity);

                if (aggregate == null)
                {
                    throw new InvalidOperationException($"Aggregate of type {typeof(TAggregate)} with Identity='{command.TargetIdentity}' does not exist.");
                }

                var modifyResult = await this.modifyHandler(command, aggregate);

                if (modifyResult.Success)
                {
                    var events = modifyResult.Value;
                    await this.publisher.Publish(events);
                    await context.Ack();
                    await OnCommandSucceeded(command, aggregate);
                }
                else
                {
                    await context.Nack(modifyResult.Error);
                    await OnCommandRejected(modifyResult.Error, command);
                }
            }
            catch (Exception ex)
            {
                await this.OnException(command, ex);
                throw;
            }

        }


    }

    public interface ICreateAggregateCommandHandler<TAggregateIdentity, TAggregate, TCommand, TError>
        where TCommand : ICreateCommand<TAggregateIdentity>
        where TAggregate : IAggregate<TAggregateIdentity>
    {
        Task<CreateAggregateCommandHandlerResult<TAggregateIdentity, TAggregate, TError>> Handle(TCommand command);
    }

    public interface IAggregateCommandHandler<TAggregateIdentity, TCommand, TAggregate, TError>
    {
        Task<AggregateCommandHandlerResult<TAggregateIdentity, TError>> Handle(TCommand command, TAggregate aggregate);
    }
}
