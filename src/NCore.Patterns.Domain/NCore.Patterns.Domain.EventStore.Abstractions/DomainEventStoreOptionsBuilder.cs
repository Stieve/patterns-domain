using Microsoft.Extensions.DependencyInjection;

namespace NCore.Patterns.Domain.EventStore.Abstractions
{
    public class DomainEventStoreOptionsBuilder
    {
        public DomainEventStoreOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}