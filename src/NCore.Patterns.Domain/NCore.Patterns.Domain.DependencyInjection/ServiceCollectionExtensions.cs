using Microsoft.Extensions.DependencyInjection;
using NCore.Patterns.Domain.Abstractions;

namespace NCore.Patterns.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultDomain(this IServiceCollection services)
        {
            services.AddScoped<IEventEmitter, DefaultEventEmitter>();
            return services;
        }
    }
}
