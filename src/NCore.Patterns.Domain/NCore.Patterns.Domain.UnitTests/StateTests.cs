using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.UnitTests.Helpers;
using NUnit.Framework;

namespace NCore.Patterns.Domain.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class StateTests
    {
        [Test]
        public void TestRegisteredHandlers()
        {
            var eventEmitterMock = new Mock<IEventEmitter>();

            var sut = new LightState(eventEmitterMock.Object);

            Assert.NotNull(sut);
            eventEmitterMock.Verify(emitter => emitter.Register(It.IsAny<Action<LightHasBeenTurnedOn>>()), Times.Once);
        }
    }
}
