using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Microsoft.Extensions.Options;
using NCore.Patterns.Domain.Abstractions;
using NCore.Patterns.Domain.EventStore.Abstractions;
using Newtonsoft.Json;

namespace NCore.Patterns.Domain.EventStore.Serialization
{
    public class DefaultDomainEventSerializer : IDomainEventSerializer
    {
        private readonly IOptions<JsonSerializerSettings> _jsonSerializerSettingsOptions;

        public DefaultDomainEventSerializer(IOptions<JsonSerializerSettings> jsonSerializerSettingsOptions)
        {
            _jsonSerializerSettingsOptions = jsonSerializerSettingsOptions;
        }

        public IDomainEvent Deserialize(ResolvedEvent resolvedEvent)
        {
            return (IDomainEvent)DeserializeAsObject(resolvedEvent);
        }

        public TDomainEvent Deserialize<TDomainEvent>(ResolvedEvent resolvedEvent) 
            where TDomainEvent : IDomainEvent
        {
            return (TDomainEvent)DeserializeAsObject(resolvedEvent);
        }

        private object DeserializeAsObject(ResolvedEvent resolvedEvent)
        {
            var eventClrTypeName = JsonConvert.DeserializeObject<Metadata>(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Metadata), _jsonSerializerSettingsOptions.Value).EventClrType;
            var typedEvent = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data), Type.GetType(eventClrTypeName), _jsonSerializerSettingsOptions.Value);
            return typedEvent;
        }

        public EventData Serialize<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettingsOptions.Value));

            var eventHeaders = new Dictionary<string, object>
            {
                {
                    nameof(Metadata.EventClrType), domainEvent.GetType().AssemblyQualifiedName
                }
            };

            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, _jsonSerializerSettingsOptions.Value));
            var typeName = domainEvent.GetType().Name;

            var eventId = Guid.NewGuid();
            return new EventData(eventId, typeName, true, data, metadata);
        }
    }
}