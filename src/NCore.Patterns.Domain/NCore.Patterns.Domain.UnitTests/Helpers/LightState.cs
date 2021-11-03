using System.Diagnostics.CodeAnalysis;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain.UnitTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class LightState : State
    {
        public LightState(IEventEmitter eventEmitter)
            : base(eventEmitter)
        {
        }

        public bool IsTurnedOn { get; private set; }

        protected override void RegisterHandlers()
        {
            On<LightHasBeenTurnedOn>(@event =>
            {
                IsTurnedOn = true;
            });
            On<LightHasBeenTurnedOff>(@event =>
            {
                IsTurnedOn = false;
            });
        }
    }
}