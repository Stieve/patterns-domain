using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using NCore.Infra.EventStore.Abstractions;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.EventStore.Abstractions;
using NCore.Patterns.Domain.EventStore.Serialization;

namespace NCore.Patterns.Domain.EventStore
{
    public class EventSourcedRepository : IEventSourcedRepository
    {
        private readonly IManagedEventStoreConnection _managedEventStoreConnection;
        private readonly IDomainEventSerializer _eventSerializer;

        public EventSourcedRepository(IManagedEventStoreConnectionFactory managedEventStoreConnectionFactory, IDomainEventSerializer eventSerializer)
            : this(managedEventStoreConnectionFactory.Create(), eventSerializer)
        {
        }

        internal EventSourcedRepository(IManagedEventStoreConnection managedEventStoreConnection, IDomainEventSerializer eventSerializer)
        {
            _managedEventStoreConnection = managedEventStoreConnection;
            _eventSerializer = eventSerializer;
        }

        public async Task<IEnumerable<IDomainEvent>> GetAsync(Type aggregateType, string id, CancellationToken cancellationToken)
        {
            var domainEvents = new List<IDomainEvent>();
            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            var streamName = GetStreamName(aggregateType, id);
            do
            {
                currentSlice = await _managedEventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);
                nextSliceStart = currentSlice.NextEventNumber;
                domainEvents.AddRange(currentSlice.Events.DeserializeEvents(_eventSerializer));
            } while (!currentSlice.IsEndOfStream);

            return domainEvents;
        }

        public async Task SaveAsync(Type aggregateType, string id, int expectedVersion, IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            var commitId = Guid.NewGuid();
            if (!domainEvents.Any())
                return;

            var streamName = GetStreamName(aggregateType, id);
            var commitHeaders = new Dictionary<string, object>
            {
                {"CommitId", commitId},
                {"AggregateClrType", aggregateType.AssemblyQualifiedName}
            };

            var eventsToSave = domainEvents.Select(e => e.ToEventData(commitHeaders, _eventSerializer)).ToList();
            await _managedEventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave);
        }

        private string GetStreamName(Type aggregateType, string id) => $"{aggregateType.Name}-{id}";
    }
}