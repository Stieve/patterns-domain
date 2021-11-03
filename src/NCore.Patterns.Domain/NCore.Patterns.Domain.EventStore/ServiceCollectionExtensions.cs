using System;
using Microsoft.Extensions.DependencyInjection;
using NCore.Patterns.Domain.EventStore.Abstractions;

namespace NCore.Patterns.Domain.EventStore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNCoreDomainEventStore(this IServiceCollection services, Action<DomainEventStoreOptionsBuilder> configure)
        {
            configure(new DomainEventStoreOptionsBuilder(services));
            //services.AddNCoreEventStore(configure);
            return services;
        }
    }
}