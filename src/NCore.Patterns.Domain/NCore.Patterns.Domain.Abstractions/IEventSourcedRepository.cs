using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NCore.Patterns.Domain.Abstractions
{
    public interface IEventSourcedRepository
    {
        Task<IEnumerable<IDomainEvent>> GetAsync(Type aggregateType, string id, CancellationToken cancellationToken);
        Task SaveAsync(Type aggregateType, string id, int expectedVersion, IEnumerable<IDomainEvent> domainEvents,
            CancellationToken cancellationToken);
    }
}