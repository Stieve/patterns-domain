using NCore.Patterns.Domain.EventStore.Abstractions;
using NCore.Patterns.Domain.EventStore.Serialization;

namespace NCore.Patterns.Domain.EventStore
{
    public static class DomainEventStoreBuilderExtensions
    {

        public static DomainEventStoreOptionsBuilder UseDefaultEventSourcedRepository(this DomainEventStoreOptionsBuilder builder)
        {
            return builder.UseEventSourcedRepository<EventSourcedRepository>();
        }

        public static DomainEventStoreOptionsBuilder UseDefaultDomainEventSerializer(this DomainEventStoreOptionsBuilder builder)
        {
            return builder.UseDomainEventSerializer<DefaultDomainEventSerializer>();
        }

        public static DomainEventStoreOptionsBuilder UseDefaultEventEmitter(this DomainEventStoreOptionsBuilder builder)
        {
            return builder.UseEventEmitter<DefaultEventEmitter>();
        }
    }
}