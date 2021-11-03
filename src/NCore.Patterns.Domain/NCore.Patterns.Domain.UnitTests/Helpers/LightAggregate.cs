using System.Diagnostics.CodeAnalysis;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain.UnitTests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class LightAggregate : Aggregate<LightState>
    {
        public LightAggregate(string key, LightState state, IEventEmitter eventEmitter) 
            : base(key, state, eventEmitter)
        {
        }

        public bool IsTurnedOn()
        {
            return State.IsTurnedOn;
        }

        public void TurnLightOn()
        {
            Handle(new LightHasBeenTurnedOn());
        }

        public void HandleNullEvent()
        {
            Handle<LightHasBeenTurnedOn>(null);
        }
    }
}