using System;

namespace TomTom.Useful.EventSourcing.UnitTests
{
    class TestEventBase : Event<TestAggregateIdentity>
    {
        protected TestEventBase(int id) : 
            base(new TestAggregateIdentity(id), -1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        {
        }
    }
}