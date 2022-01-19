namespace TomTom.Useful.EventSourcing.UnitTests
{
    class TestEventOne : TestEventBase
    {
        public TestEventOne(int id, string eventOneValue) : base(id)
        {
            EventOneValue = eventOneValue;
        }

        public string EventOneValue { get; }
    }
}