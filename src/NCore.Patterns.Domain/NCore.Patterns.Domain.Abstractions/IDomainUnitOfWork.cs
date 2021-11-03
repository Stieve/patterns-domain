using System.Threading;
using System.Threading.Tasks;

namespace NCore.Patterns.Domain.Abstractions
{
    public interface IDomainUnitOfWork<TAggregate>
        where TAggregate : IAggregate
    {
        Task<TAggregate> GetAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState;
        Task<TAggregate> GetNewAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState;
        Task<TAggregate> GetExistingAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState;
        Task CommitAsync(int expectedVersion, CancellationToken cancellationToken);
    }
}