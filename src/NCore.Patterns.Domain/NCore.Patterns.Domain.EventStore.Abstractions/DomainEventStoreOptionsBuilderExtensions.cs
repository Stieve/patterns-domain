using Microsoft.Extensions.DependencyInjection;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain.EventStore.Abstractions
{
    public static class DomainEventStoreOptionsBuilderExtensions
    {
        public static DomainEventStoreOptionsBuilder UseEventSourcedRepository<TEventSourcedRepository>(this DomainEventStoreOptionsBuilder builder)
            where TEventSourcedRepository : class, IEventSourcedRepository
        {
            builder.Services.AddScoped<IEventSourcedRepository, TEventSourcedRepository>();
            return builder;
        }

        public static DomainEventStoreOptionsBuilder UseDomainEventSerializer<TDomainEventSerializer>(this DomainEventStoreOptionsBuilder builder)
            where TDomainEventSerializer : class, IDomainEventSerializer
        {
            builder.Services.AddSingleton<IDomainEventSerializer, TDomainEventSerializer>();
            return builder;
        }

        public static DomainEventStoreOptionsBuilder UseEventEmitter<TEventEmitter>(this DomainEventStoreOptionsBuilder builder)
            where TEventEmitter : class, IEventEmitter
        {
            builder.Services.AddScoped<IEventEmitter, TEventEmitter>();
            return builder;
        }
    }
}