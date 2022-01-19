namespace TomTom.Useful.EventSourcing.UnitTests
{
    class TestAggregateIdentity
    {
        public TestAggregateIdentity(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}