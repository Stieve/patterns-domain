using EventStore.ClientAPI;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain.EventStore.Abstractions
{
    public interface IDomainEventSerializer
    {
        IDomainEvent Deserialize(ResolvedEvent resolvedEvent);
        TDomainEvent Deserialize<TDomainEvent>(ResolvedEvent resolvedEvent)
            where TDomainEvent : IDomainEvent;
        EventData Serialize<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent;
    }
}
