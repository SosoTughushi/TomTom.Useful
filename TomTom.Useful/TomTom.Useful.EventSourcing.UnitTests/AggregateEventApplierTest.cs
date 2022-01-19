using System.Collections.Generic;
using Xunit;

namespace TomTom.Useful.EventSourcing.UnitTests
{
    public class AggregateEventApplierTest
    {
        [Fact]
        public void ApplyEvents_ShouldApplyDefinedEvents()
        {
            // arrange
            var aggregate = new TestAggregate();

            var events = new List<Event<TestAggregateIdentity>>
            {
                new TestEventOne(1,"InitialOne"),
                new TestEventTwo(1, "InitialTwo"),
                new TestEventOne(1,"LastOne"),
                new TestEventTwo(1,"LastTwo"),
            };

            // act
            AggregateEventApplier<TestAggregate>.ApplyEvents(aggregate, events);

            // assert
            Assert.Equal("LastOne", aggregate.ValueOne);
            Assert.Equal("LastTwo", aggregate.ValueTwo);
        }

    }
}