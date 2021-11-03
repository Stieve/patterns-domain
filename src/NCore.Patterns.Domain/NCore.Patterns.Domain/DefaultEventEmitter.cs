using System;
using System.Collections.Generic;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.Abstractions.Exceptions;

namespace NCore.Patterns.Domain
{
    public class DefaultEventEmitter : IEventEmitter
    {
        private readonly IDictionary<Type, List<Action<IDomainEvent>>> _actions = new Dictionary<Type, List<Action<IDomainEvent>>>();

        public virtual void Register<TEvent>(Action<TEvent> action)
            where TEvent : IDomainEvent, new()
        {
            void ActionToRegister(IDomainEvent domainEvent)
            {
                var typedDomainEvent = (TEvent)domainEvent;
                action(typedDomainEvent);
            }

            if (_actions.ContainsKey(typeof(TEvent)))
            {
                _actions[typeof(TEvent)].Add(ActionToRegister);
            }
            else
            {
                _actions.Add(typeof(TEvent), new List<Action<IDomainEvent>> { ActionToRegister });
            }
        }

        public void Emit(IEnumerable<IDomainEvent> events)
        {
            if (events == null)
                throw new DomainEventException("The list of emitted domainevent is null");

            foreach (var domainEvent in events)
            {
                Emit(domainEvent);
            }
        }

        public void Emit(IDomainEvent @event)
        {
            if (@event == null)
                throw new DomainEventException("The emitted domainevent is null");

            var eventType = @event.GetType();
            if (!_actions.ContainsKey(eventType))
                throw new InvalidOperationException($"There are no actions registered to handle {eventType}");

            foreach (var action in _actions[eventType])
            {
                action(@event);
            }
        }
    }
}