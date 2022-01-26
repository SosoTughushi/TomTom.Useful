using Automatonymous;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Futures;
using MassTransit.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace TomTom.Useful.Messaging.MassTransit
{
    public class MassTransitCollectionBusConfiguratorDecorator : IServiceCollectionBusConfigurator
    {
        private readonly IServiceCollectionBusConfigurator decorated;

        public MassTransitCollectionBusConfiguratorDecorator(IServiceCollectionBusConfigurator decorated)
        {
            this.decorated = decorated;
        }

        public IServiceCollection Collection => decorated.Collection;

        public IContainerRegistrar Registrar => decorated.Registrar;

        public IActivityRegistrationConfigurator AddActivity(Type activityType, Type activityDefinitionType = null)
        {
            return decorated.AddActivity(activityType, activityDefinitionType);
        }

        public void AddBus(Func<IBusRegistrationContext, IBusControl> busFactory)
        {
            decorated.AddBus(busFactory);
        }

        public void AddConfigureEndpointsCallback(ConfigureEndpointsCallback callback)
        {
            decorated.AddConfigureEndpointsCallback(callback);
        }

        public IConsumerRegistrationConfigurator AddConsumer(Type consumerType, Type consumerDefinitionType = null)
        {
            return decorated.AddConsumer(consumerType, consumerDefinitionType);
        }

        public void AddEndpoint(Type endpointDefinition)
        {
            decorated.AddEndpoint(endpointDefinition);
        }

        public IExecuteActivityRegistrationConfigurator AddExecuteActivity(Type activityType, Type activityDefinitionType)
        {
            return decorated.AddExecuteActivity(activityType, activityDefinitionType);
        }

        public IFutureRegistrationConfigurator<TFuture> AddFuture<TFuture>(Type futureDefinitionType = null) where TFuture : MassTransitStateMachine<FutureState>
        {
            return decorated.AddFuture<TFuture>(futureDefinitionType);
        }

        public IFutureRegistrationConfigurator AddFuture(Type futureType, Type futureDefinitionType = null)
        {
            return decorated.AddFuture(futureType, futureDefinitionType);
        }

        public void AddMessageScheduler(IMessageSchedulerRegistration registration)
        {
            decorated.AddMessageScheduler(registration);
        }

        public void AddRequestClient<T>(RequestTimeout timeout = default) where T : class
        {
            decorated.AddRequestClient<T>(timeout);
        }

        public void AddRequestClient<T>(Uri destinationAddress, RequestTimeout timeout = default) where T : class
        {
            decorated.AddRequestClient<T>(destinationAddress, timeout);
        }

        public void AddRequestClient(Type requestType, RequestTimeout timeout = default)
        {
            decorated.AddRequestClient(requestType, timeout);
        }

        public void AddRequestClient(Type requestType, Uri destinationAddress, RequestTimeout timeout = default)
        {
            decorated.AddRequestClient(requestType, destinationAddress, timeout);
        }

        public void AddRider(Action<IRiderRegistrationConfigurator> configure)
        {
            decorated.AddRider(configure);

        }

        public ISagaRegistrationConfigurator AddSaga(Type sagaType, Type sagaDefinitionType = null)
        {
            return decorated.AddSaga(sagaType, sagaDefinitionType);
        }

        public void AddSagaStateMachine(Type sagaType, Type sagaDefinitionType = null)
        {
            decorated.AddSagaStateMachine(sagaType, sagaDefinitionType);
        }

        public void SetBusFactory<T>(T busFactory) where T : IRegistrationBusFactory
        {
            decorated.SetBusFactory<T>(busFactory);
        }

        public void SetEndpointNameFormatter(IEndpointNameFormatter endpointNameFormatter)
        {
            decorated.SetEndpointNameFormatter(endpointNameFormatter);
        }

        public void SetSagaRepositoryProvider(ISagaRepositoryRegistrationProvider provider)
        {
            decorated.SetSagaRepositoryProvider(provider);
        }

        IActivityRegistrationConfigurator<TActivity, TArguments, TLog> IRegistrationConfigurator.AddActivity<TActivity, TArguments, TLog>(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configureExecute, Action<ICompensateActivityConfigurator<TActivity, TLog>> configureCompensate)
        {
            return ((IRegistrationConfigurator)decorated).AddActivity<TActivity, TArguments, TLog>(configureExecute, configureCompensate);
        }

        IActivityRegistrationConfigurator<TActivity, TArguments, TLog> IRegistrationConfigurator.AddActivity<TActivity, TArguments, TLog>(
            Type activityDefinitionType, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configureExecute
            , Action<ICompensateActivityConfigurator<TActivity, TLog>> configureCompensate)
        {
            return ((IRegistrationConfigurator)decorated).AddActivity<TActivity, TArguments, TLog>(configureExecute, configureCompensate);
        }

        IConsumerRegistrationConfigurator<T> IRegistrationConfigurator.AddConsumer<T>(Action<IConsumerConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddConsumer(configure);
        }

        IConsumerRegistrationConfigurator<T> IRegistrationConfigurator.AddConsumer<T>(Type consumerDefinitionType, Action<IConsumerConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddConsumer(consumerDefinitionType, configure);
        }

        void IRegistrationConfigurator.AddEndpoint<TDefinition, T>(IEndpointSettings<IEndpointDefinition<T>> settings)
        {
            ((IRegistrationConfigurator)decorated).AddEndpoint<TDefinition, T>(settings);
        }

        IExecuteActivityRegistrationConfigurator<TActivity, TArguments> IRegistrationConfigurator.AddExecuteActivity<TActivity, TArguments>(
            Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddExecuteActivity<TActivity, TArguments>(configure);
        }

        IExecuteActivityRegistrationConfigurator<TActivity, TArguments> IRegistrationConfigurator.AddExecuteActivity<TActivity, TArguments>(
            Type executeActivityDefinitionType
            , Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddExecuteActivity<TActivity, TArguments>(executeActivityDefinitionType, configure);
        }

        ISagaRegistrationConfigurator<T> IRegistrationConfigurator.AddSaga<T>(Action<ISagaConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddSaga<T>(configure);
        }

        ISagaRegistrationConfigurator<T> IRegistrationConfigurator.AddSaga<T>(Type sagaDefinitionType, Action<ISagaConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddSaga<T>(sagaDefinitionType, configure);
        }

        ISagaRegistrationConfigurator<T> IRegistrationConfigurator.AddSagaRepository<T>()
        {
            return ((IRegistrationConfigurator)decorated).AddSagaRepository<T>();
        }

        ISagaRegistrationConfigurator<T> IRegistrationConfigurator.AddSagaStateMachine<TStateMachine, T>(Action<ISagaConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddSagaStateMachine<TStateMachine, T>(configure);
        }

        ISagaRegistrationConfigurator<T> IRegistrationConfigurator.AddSagaStateMachine<TStateMachine, T>(
            Type sagaDefinitionType
            , Action<ISagaConfigurator<T>> configure)
        {
            return ((IRegistrationConfigurator)decorated).AddSagaStateMachine<TStateMachine, T>(sagaDefinitionType, configure);
        }
    }
}
