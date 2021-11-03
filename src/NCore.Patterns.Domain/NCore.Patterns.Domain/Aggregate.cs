using System.Collections.Generic;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.Abstractions.Exceptions;

namespace NCore.Patterns.Domain
{
    public abstract class Aggregate<TState> : IAggregate
        where TState : State
    {
        private readonly IEventEmitter _eventEmitter;
        private readonly List<IDomainEvent> _uncommittedDomainEvents = new List<IDomainEvent>();

        protected Aggregate(string key, TState state, IEventEmitter eventEmitter)
        {
            _eventEmitter = eventEmitter;
            State = state;
            Key = key;
        }

        public string Key { get; }

        public void ClearUncommittedEvents()
        {
            _uncommittedDomainEvents.Clear();
        }
        
        protected TState State { get; }

        public IEnumerable<IDomainEvent> GetUncommittedEvents()
        {
            return _uncommittedDomainEvents;
        }

        protected void Handle<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IDomainEvent, new()
        {
            if (@event == null)
                throw new DomainEventException("The domainevent cannot be null");

            _uncommittedDomainEvents.Add(@event);
            _eventEmitter.Emit(@event);
        }
    }
}
