using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.EventStore.Abstractions;

namespace NCore.Patterns.Domain.EventStore.Serialization
{
    public static class DefaultDomainEventSerializerExtensions
    {
        public static IEnumerable<IDomainEvent> DeserializeEvents(this IEnumerable<ResolvedEvent> resolvedEvents, IDomainEventSerializer domainEventSerializer)
            => resolvedEvents.Select(resolvedEvent => DeserializeEvent(resolvedEvent, domainEventSerializer));
        
        public static IDomainEvent DeserializeEvent(this ResolvedEvent resolvedEvent, IDomainEventSerializer domainEventSerializer)
            => domainEventSerializer.Deserialize(resolvedEvent);

        public static TDomainEvent DeserializeEvent<TDomainEvent>(this ResolvedEvent resolvedEvent, IDomainEventSerializer domainEventSerializer)
            where TDomainEvent : IDomainEvent
            => domainEventSerializer.Deserialize<TDomainEvent>(resolvedEvent);

        public static EventData ToEventData<TDomainEvent>(this TDomainEvent domainEvent, IDictionary<string, object> headers, IDomainEventSerializer domainEventSerializer)
            where TDomainEvent : IDomainEvent
        {
            return domainEventSerializer.Serialize(domainEvent);
        }
    }
}