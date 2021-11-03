using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NCore.Patterns.Domain.Abstractions.Exceptions;
using NCore.Patterns.Domain.UnitTests.Helpers;
using NUnit.Framework;

namespace NCore.Patterns.Domain.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class AggregateTests
    {
        [Test]
        public void TestConstructor()
        {
            const string name = "Light Bathroom";
            var eventEmitter = new DefaultEventEmitter();
            var sut = new LightAggregate(name, new LightState(eventEmitter), eventEmitter);

            sut.Key.Should().Be(name);
        }

        [Test]
        public void TestState()
        {
            const string name = "Light Bathroom";
            var eventEmitter = new DefaultEventEmitter();
            var state = new LightState(eventEmitter);

            var sut = new LightAggregate(name, state, eventEmitter);
            sut.TurnLightOn();

            sut.IsTurnedOn().Should().BeTrue();
        }

        [Test]
        public void TestNullEvent()
        {
            Assert.Throws<DomainEventException>(() =>
            {
                const string name = "Light Bathroom";
                var eventEmitter = new DefaultEventEmitter();
                var state = new LightState(eventEmitter);

                var sut = new LightAggregate(name, state, eventEmitter);
                sut.HandleNullEvent();

            });
        }

        [Test]
        public void TestGetUncomittedEvents()
        {
            const string name = "Light Bathroom";
            var eventEmitter = new DefaultEventEmitter();
            var state = new LightState(eventEmitter);

            var sut = new LightAggregate(name, state, eventEmitter);
            sut.TurnLightOn();

            sut.GetUncommittedEvents().Should().HaveCount(1);
        }

        [Test]
        public void TestClearUncomittedEvents()
        {
            const string name = "Light Bathroom";
            var eventEmitter = new DefaultEventEmitter();
            var state = new LightState(eventEmitter);

            var sut = new LightAggregate(name, state, eventEmitter);
            sut.TurnLightOn();

            sut.GetUncommittedEvents().Should().HaveCount(1);

            sut.ClearUncommittedEvents();

            sut.GetUncommittedEvents().Should().BeEmpty();
        }
    }
}