using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.Abstractions.Exceptions;

namespace NCore.Patterns.Domain.EventStore
{
    public class EventStoreDomainUnitOfWork<TAggregate> : IDomainUnitOfWork<TAggregate>, IDisposable
        where TAggregate : IAggregate
    {
        private readonly List<IAggregate> _aggregates = new List<IAggregate>();
        private readonly IEventSourcedRepository _eventSourcedRepository;
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _scope;

        public EventStoreDomainUnitOfWork(IEventSourcedRepository eventSourcedRepository, IServiceProvider serviceProvider)
        {
            _eventSourcedRepository = eventSourcedRepository;
            _serviceProvider = serviceProvider;
        }

        public Task<TAggregate> GetAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState
        {
            return GetAsync<TState>(key, onlyNew: false, onlyExisting: false, allowDeleted: false, cancellationToken: cancellationToken);
        }

        public Task<TAggregate> GetNewAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState
        {
            return GetAsync<TState>(key, onlyNew: true, onlyExisting: false, allowDeleted: false, cancellationToken: cancellationToken);
        }

        public Task<TAggregate> GetExistingAsync<TState>(string key, CancellationToken cancellationToken)
            where TState : IState
        {
            return GetAsync<TState>(key, onlyNew: false, onlyExisting: true, allowDeleted: false, cancellationToken: cancellationToken);
        }

        private async Task<TAggregate> GetAsync<TState>(string key, bool onlyNew, bool onlyExisting, bool allowDeleted, CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateScope();
            var domainEvents = AsList(await _eventSourcedRepository.GetAsync(typeof(TAggregate), key, cancellationToken));

            if (onlyNew && domainEvents.Any())
            {
                throw new AggregateAlreadyExistsException(aggregateIdentifier: key);
            }

            if (onlyExisting && !domainEvents.Any())
            {
                return default;
            }

            var eventEmitter = _serviceProvider.GetRequiredService<IEventEmitter>();
            var state = (TState)Activator.CreateInstance(typeof(TState), eventEmitter);
            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), key, state, eventEmitter);

            eventEmitter.Emit(domainEvents);

            _aggregates.Add(aggregate);
            return aggregate;
        }

        public async Task CommitAsync(int expectedVersion, CancellationToken cancellationToken)
        {
            foreach (var aggregate in _aggregates)
            {
                var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();
                await _eventSourcedRepository.SaveAsync(aggregate.GetType(), aggregate.Key, expectedVersion, uncommittedEvents, cancellationToken);
                aggregate.ClearUncommittedEvents();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scope?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static List<T> AsList<T>(IEnumerable<T> source) => (source == null || source is List<T>) ? (List<T>)source : source.ToList();
    }
}
