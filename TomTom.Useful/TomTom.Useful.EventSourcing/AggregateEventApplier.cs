namespace TomTom.Useful.EventSourcing
{
    public static class AggregateEventApplier<TAggregate> where TAggregate : IAggregate
    {
        private static readonly Dictionary<Type, System.Reflection.MethodInfo> ApplyMethods;
        static AggregateEventApplier()
        {
            var emittedEventTypes = typeof(TAggregate).GetInterfaces()
                .Where(t => t.IsConstructedGenericType)
                .Where(t => t.Name == typeof(IEmitsEvent<>).Name)
                .SelectMany(t => t.GetGenericArguments());

            ApplyMethods = emittedEventTypes
                .ToDictionary(
                type => type,
                type => typeof(TAggregate).GetMethod(nameof(IEmitsEvent<Event>.Apply), new Type[] { type }));


        }

        public static void ApplyEvents(TAggregate aggregate, IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                var methodInfo = ApplyMethods[@event.GetType()];

                methodInfo.Invoke(aggregate, new object[] { @event });
            }
        }
    }


}
