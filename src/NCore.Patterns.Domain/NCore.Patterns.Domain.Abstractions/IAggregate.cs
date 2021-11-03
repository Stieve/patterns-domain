using System.Collections.Generic;

namespace NCore.Patterns.Domain.Abstractions
{
    public interface IAggregate
    {
        string Key { get; }
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}