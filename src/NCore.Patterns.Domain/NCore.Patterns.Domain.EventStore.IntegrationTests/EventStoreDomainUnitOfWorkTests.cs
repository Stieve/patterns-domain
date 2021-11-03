using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NCore.Infra.EventStore;
using NCore.Infra.EventStore.Abstractions;
using NCore.Infra.EventStore.InMemory;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.EventStore.Abstractions;
using NCore.Patterns.Domain.EventStore.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NCore.Patterns.Domain.EventStore.IntegrationTests
{
    public class EventStoreDomainUnitOfWorkTests
    {
        private IEventSourcedRepository _repository;
        private IServiceProvider _serviceProvider;
        private IManagedEventStoreConnection _managedEventStoreConnection;
        private IDomainEventSerializer _domainEventSerializer;

        [SetUp]
        public async Task Setup()
        {
            var store = new InMemoryEventStore();;

            await store.Node.StartAsync(true);

            _managedEventStoreConnection = ManagedEventStoreConnection.Create(EmbeddedEventStoreConnection.Create(store.Node));

            var optionsMock = new Mock<IOptions<JsonSerializerSettings>>();
            optionsMock.SetupGet(o => o.Value).Returns(new JsonSerializerSettings());
            _repository = new EventSourcedRepository(_managedEventStoreConnection, _domainEventSerializer = new DefaultDomainEventSerializer(optionsMock.Object));

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IEventEmitter, DefaultEventEmitter>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public async Task TestGetNewAsync_ShouldSucceed()
        {
            //Arrange
            var key = Guid.NewGuid().ToString("N");
            var sut = new EventStoreDomainUnitOfWork<SomeAggregate>(_repository, _serviceProvider);

            //Act
            var aggregate = await sut.GetNewAsync<SomeState>(key, CancellationToken.None);

            //Assert
            aggregate.Should().NotBeNull();
            aggregate.Key.Should().Be(key);
        }

        [Test]
        public async Task TestGetExistingAsync_WhenNotExists_ShouldBeNull()
        {
            //Arrange
            var key = Guid.NewGuid().ToString("N");
            var sut = new EventStoreDomainUnitOfWork<SomeAggregate>(_repository, _serviceProvider);

            //Act
            var aggregate = await sut.GetExistingAsync<SomeState>(key, CancellationToken.None);

            //Assert
            aggregate.Should().BeNull();
        }

        [Test]
        public async Task TestGetExistingAsync_ShouldSucceed()
        {
            //Arrange
            var key = Guid.NewGuid().ToString("N");
            //Create new stream
            var eventData = _domainEventSerializer.Serialize(new SomethingHappened());
            await _managedEventStoreConnection.AppendToStreamAsync($"{nameof(SomeAggregate)}-{key}", ExpectedVersion.NoStream, eventData);
            var sut = new EventStoreDomainUnitOfWork<SomeAggregate>(_repository, _serviceProvider);

            //Act
            var aggregate = await sut.GetExistingAsync<SomeState>(key, CancellationToken.None);

            //Assert
            aggregate.Should().NotBeNull();
            aggregate.Key.Should().Be(key);
        }

        [Test]
        public async Task TestCommitAsync_ShouldSucceed()
        {
            //Arrange
            var key = Guid.NewGuid().ToString("N");
            var sut = new EventStoreDomainUnitOfWork<SomeAggregate>(_repository, _serviceProvider);
            var arrangeAggregate = await sut.GetNewAsync<SomeState>(key, CancellationToken.None);
            arrangeAggregate.LetSomethingHappen();

            //Act
            await sut.CommitAsync(ExpectedVersion.Any, CancellationToken.None);

            //Assert
            var assertSut = new EventStoreDomainUnitOfWork<SomeAggregate>(_repository, _serviceProvider);
            var assertAggregate = await assertSut.GetExistingAsync<SomeState>(key, CancellationToken.None);

            assertAggregate.Should().NotBeNull();
            assertAggregate.Key.Should().Be(key);
        }
    }

    public class SomeAggregate : Aggregate<SomeState>
    {
        public SomeAggregate(string key, SomeState state, IEventEmitter eventEmitter)
            : base(key, state, eventEmitter)
        {
        }

        public void LetSomethingHappen()
        {
            Handle(new SomethingHappened());
        }
    }

    public class SomeState : State
    {
        public SomeState(IEventEmitter eventEmitter)
            : base(eventEmitter)
        {
        }

        protected override void RegisterHandlers()
        {
            On<SomethingHappened>(@event => { });
        }
    }

    public class SomethingHappened : IDomainEvent
    {
    }
}