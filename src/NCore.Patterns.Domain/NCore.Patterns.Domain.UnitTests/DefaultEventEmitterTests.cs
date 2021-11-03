using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Moq;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.Abstractions.Exceptions;
using NCore.Patterns.Domain.UnitTests.Helpers;
using NUnit.Framework;

namespace NCore.Patterns.Domain.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class DefaultEventEmitterTests
    {
        [Test]
        public void TestRegisterAndEmit()
        {
            var lightHasBeenTurnedOnEventActionMock = new Mock<Action<LightHasBeenTurnedOn>>();
            var sut = new DefaultEventEmitter();
            var @event = new LightHasBeenTurnedOn();

            sut.Register(lightHasBeenTurnedOnEventActionMock.Object);
            sut.Emit(@event);
            lightHasBeenTurnedOnEventActionMock.Verify(action => action.Invoke(@event), Times.Once);
        }

        [Test]
        public void TestRegisterMultipleActionsForSameEventAndEmit()
        {
            var lightHasBeenTurnedOnEventActionMock1 = new Mock<Action<LightHasBeenTurnedOn>>();
            var lightHasBeenTurnedOnEventActionMock2 = new Mock<Action<LightHasBeenTurnedOn>>();
            var sut = new DefaultEventEmitter();
            var @event = new LightHasBeenTurnedOn();

            sut.Register(lightHasBeenTurnedOnEventActionMock1.Object);
            sut.Register(lightHasBeenTurnedOnEventActionMock2.Object);
            sut.Emit(@event);
            lightHasBeenTurnedOnEventActionMock1.Verify(action => action.Invoke(@event), Times.Once);
            lightHasBeenTurnedOnEventActionMock2.Verify(action => action.Invoke(@event), Times.Once);
        }

        [Test]
        public void TestRegisterAndEmitWithUnregisteredEvent()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var lightHasBeenTurnedOnEventActionMock = new Mock<Action<LightHasBeenTurnedOn>>();
                var sut = new DefaultEventEmitter();
                var @event = new LightHasBeenTurnedOff();

                sut.Register(lightHasBeenTurnedOnEventActionMock.Object);
                sut.Emit(@event);
            });
        }

        [Test]
        public void TestRegisterAndEmitNullEvent()
        {
            Assert.Throws<DomainEventException>(() =>
            {
                var lightHasBeenTurnedOnEventActionMock = new Mock<Action<LightHasBeenTurnedOn>>();
                var sut = new DefaultEventEmitter();

                sut.Register(lightHasBeenTurnedOnEventActionMock.Object);
                sut.Emit((IDomainEvent)null);
            });
        }

        [Test]
        public void TestRegisterAndEmitMultipleEvents()
        {
            var lightHasBeenTurnedOnEventActionMock = new Mock<Action<LightHasBeenTurnedOn>>();
            var sut = new DefaultEventEmitter();

            var @event = new LightHasBeenTurnedOn();

            sut.Register(lightHasBeenTurnedOnEventActionMock.Object);
            sut.Emit(Enumerable.Repeat<IDomainEvent>(@event, 5));

            lightHasBeenTurnedOnEventActionMock.Verify(action => action.Invoke(@event), Times.Exactly(5));
        }

        [Test]
        public void TestRegisterAndEmitMultipleNullEvents()
        {
            Assert.Throws<DomainEventException>(() =>
            {
                var lightHasBeenTurnedOnEventActionMock = new Mock<Action<LightHasBeenTurnedOn>>();
                var sut = new DefaultEventEmitter();

                sut.Register(lightHasBeenTurnedOnEventActionMock.Object);
                sut.Emit((IEnumerable<IDomainEvent>)null);
            });
        }
    }
}