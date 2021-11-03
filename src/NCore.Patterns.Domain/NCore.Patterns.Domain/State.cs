using System;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain
{
    public abstract class State : IState
    {
        private readonly IEventEmitter _eventEmitter;

        protected State(IEventEmitter eventEmitter)
        {
            _eventEmitter = eventEmitter;
#pragma warning disable S1699 // Constructors should only call non-overridable methods
            // ReSharper disable once VirtualMemberCallInConstructor
            RegisterHandlers();
#pragma warning restore S1699 // Constructors should only call non-overridable methods
        }

        protected abstract void RegisterHandlers();

        protected void On<TDomainEvent>(Action<TDomainEvent> action) where TDomainEvent : IDomainEvent, new()
        {
            _eventEmitter.Register(action);
        }
    }
}