namespace TomTom.Useful.EventSourcing
{
    public interface IEmitsEvent<T> where T: Event
    {
        void Apply(T @event);
    }
}
