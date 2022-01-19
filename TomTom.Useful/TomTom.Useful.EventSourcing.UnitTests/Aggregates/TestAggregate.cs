namespace TomTom.Useful.EventSourcing.UnitTests
{
    class TestAggregate : IAggregate<TestAggregateIdentity>, IEmitsEvent<TestEventOne>, IEmitsEvent<TestEventTwo>
    {
        public long Version { get; set; }

        public TestAggregateIdentity Id { get; }
        public string ValueOne { get; private set; }
        public string ValueTwo { get; private set; }

        public void Apply(TestEventOne @event)
        {
            this.ValueOne = @event.EventOneValue;
        }

        public void Apply(TestEventTwo @event)
        {
            this.ValueTwo = @event.TestEventValue2;
        }
    }
}