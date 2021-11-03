using System;
using System.Collections.Generic;

namespace NCore.Patterns.Domain.Abstractions
{
    public interface IEventEmitter
    {
        void Register<TEvent>(Action<TEvent> action)
            where TEvent : IDomainEvent, new();
        void Emit(IEnumerable<IDomainEvent> events);
        void Emit(IDomainEvent @event);
    }
}