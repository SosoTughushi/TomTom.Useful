namespace TomTom.Useful.EventSourcing.UnitTests
{
    class TestEventTwo : TestEventBase
    {
        public TestEventTwo(int id, string testEventValue2) : base(id)
        {
            TestEventValue2 = testEventValue2;
        }

        public string TestEventValue2 { get; }
    }
}